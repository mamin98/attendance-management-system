using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceCalculationService(IUnitOfWork unitOfWork) : IAttendanceCalculationService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<MonthlyAttendanceReportDto>> GenerateMonthlyReportAsync(GenerateReportRequestDto request)
    {
        List<Employee> employees = await ResolveTargetEmployeesAsync(request);

        List<MonthlyAttendanceReportDto> reports = [];

        foreach (Employee employee in employees)
            reports.Add(await BuildReportForEmployeeAsync(employee, request.Year, request.Month));

        return reports;
    }

    private async Task<List<Employee>> ResolveTargetEmployeesAsync(GenerateReportRequestDto request)
    {
        if (request.EmployeeId.HasValue)
        {
            Employee? employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId.Value)
                ?? throw new NotFoundException("Employee not found");

            return [employee];
        }

        IReadOnlyList<Employee> all = await _unitOfWork.EmployeeRepository.GetAllAsync();

        if (request.DepartmentId.HasValue)
            return [.. all.Where(e => e.EmployeeDepartments
                .Any(ed => ed.DepartmentId == request.DepartmentId && ed.EndDate == null))];

        return [.. all];
    }

    private async Task<MonthlyAttendanceReportDto> BuildReportForEmployeeAsync(Employee employee, int year, int month)
    {
        DateTime monthStart = new(year, month, 1);
        DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

        IReadOnlyList<AttendanceLog> logs = await _unitOfWork.AttendanceLogRepository
            .GetByEmployeeAndMonthAsync(employee.Id, year, month);

        IReadOnlyList<EmployeeShift> shiftAssignments = await _unitOfWork.EmployeeShiftRepository
            .GetByEmployeeIdAsync(employee.Id);

        List<LeaveRequest> approvedLeaves = [.. (await _unitOfWork.LeaveRequestRepository
            .GetEmployeeRequestsAsync(employee.Id))
            .Where(x => x.Status == LeaveRequestStatus.Approved
                && x.StartDate <= monthEnd && x.EndDate >= monthStart)];

        List<AttendanceRequest> approvedRequests = [.. (await _unitOfWork.AttendanceRequestRepository
            .GetEmployeeRequestsAsync(employee.Id))
            .Where(x => x.RequestStatus == RequestStatus.Approved
                && x.RequestDate >= monthStart && x.RequestDate <= monthEnd)];

        AttendancePolicy? policy = await ResolvePolicyAsync(employee.Id);

        MonthlyAttendanceReportDto report = new()
        {
            EmployeeData = employee.ToSimpleDto(),
            Month = month,
            Year = year
        };

        for (DateTime date = monthStart; date <= monthEnd; date = date.AddDays(1))
        {
            DailyAttendanceDetailDto detail = await BuildDailyDetailAsync(
                date, logs, shiftAssignments, approvedLeaves, approvedRequests);

            report.Days.Add(detail);

            if (!detail.IsWorkingDay) continue;

            report.WorkingDaysInMonth++;

            switch (detail.Status)
            {
                case "Present": report.PresentDays++; break;
                case "Absent": report.AbsentDays++; break;
                case "OnLeave": report.ApprovedLeaveDays++; break;
                case "Remote": report.ApprovedRemoteDays++; break;
                case "Permission": report.ApprovedPermissionDays++; break;
            }

            report.TotalWorkedHours += detail.WorkedHours;

            if (detail.LateMinutes > 0)
            {
                report.LateDaysCount++;
                report.TotalLateMinutes += detail.LateMinutes;
            }

            if (detail.EarlyLeaveMinutes > 0)
            {
                report.EarlyLeaveDaysCount++;
                report.TotalEarlyLeaveMinutes += detail.EarlyLeaveMinutes;
            }
        }

        if (policy is not null)
        {
            report.LatePolicyExceeded = report.TotalLateMinutes > policy.MaxLateMinutesPerMonth;
            report.EarlyLeavePolicyExceeded = report.EarlyLeaveDaysCount > policy.MaxEarlyLeavesPerMonth;
        }

        return report;
    }

    private async Task<DailyAttendanceDetailDto> BuildDailyDetailAsync(
        DateTime date,
        IReadOnlyList<AttendanceLog> logs,
        IReadOnlyList<EmployeeShift> shiftAssignments,
        List<LeaveRequest> approvedLeaves,
        List<AttendanceRequest> approvedRequests)
    {
        DailyAttendanceDetailDto detail = new() { Date = date };

        EmployeeShift? activeAssignment = shiftAssignments
            .FirstOrDefault(x => x.StartDate <= date && (x.EndDate == null || x.EndDate >= date));

        if (activeAssignment?.ShiftId is null)
        {
            detail.IsWorkingDay = false;
            detail.Status = AttendanceStatus.NoShiftAssigned.ToString();
            return detail;
        }

        Shift? shift = await _unitOfWork.ShiftRepository.GetByIdAsync(activeAssignment.ShiftId.Value);
        ShiftDay? shiftDay = shift?.ShiftDays.FirstOrDefault(x => x.Day == DateOnly.FromDateTime(date));

        if (shiftDay is null || shiftDay.IsOffDay)
        {
            detail.IsWorkingDay = false;
            detail.Status = AttendanceStatus.OffDay.ToString();
            return detail;
        }

        detail.IsWorkingDay = true;

        ShiftDayDetail? firstDetail = shiftDay.ShiftDayDetails?.OrderBy(x => x.From).FirstOrDefault();
        ShiftDayDetail? lastDetail = shiftDay.ShiftDayDetails?.OrderByDescending(x => x.To).FirstOrDefault();

        detail.ExpectedStart = firstDetail?.From.ToString();
        detail.ExpectedEnd = lastDetail?.To.ToString();

        AttendanceLog? log = logs.FirstOrDefault(x => x.Date.Date == date.Date);

        if (log?.CheckIn is not null)
        {
            detail.ActualCheckIn = log.CheckIn.ToString();
            detail.ActualCheckOut = log.CheckOut.ToString();

            if (!string.IsNullOrEmpty(detail.ExpectedStart))
            {
                TimeSpan checkInValue = log.CheckIn.Value.ToTimeSpan();
                TimeSpan latestAllowed = TimeSpan.Parse(detail.ExpectedStart).Add(TimeSpan.FromMinutes(shift!.GracePeriodMinutes));

                if (checkInValue > latestAllowed)
                    detail.LateMinutes = (int)(checkInValue - latestAllowed).TotalMinutes;
            }
            TimeSpan checkOutValue = log.CheckOut!.Value.ToTimeSpan();
            TimeSpan expectedEndValue = TimeSpan.Parse(detail.ExpectedEnd!);

            if (!string.IsNullOrEmpty(detail.ExpectedEnd) && checkOutValue < expectedEndValue)
                detail.EarlyLeaveMinutes = (int)(expectedEndValue - checkOutValue).TotalMinutes;

            if (log.CheckOut.HasValue)
                detail.WorkedHours = (decimal)(log.CheckOut.Value - log.CheckIn.Value).TotalHours;

            detail.Status = AttendanceStatus.Present.ToString();
            return detail;
        }

        if (approvedLeaves.Any(x => x.StartDate <= date && x.EndDate >= date))
        {
            detail.Status = "OnLeave";
            return detail;
        }

        AttendanceRequest? coveringRequest = approvedRequests.FirstOrDefault(x => x.RequestDate.Date == date.Date);
        if (coveringRequest is not null)
        {
            detail.Status = coveringRequest.RequestType switch
            {
                RequestType.Remote => "Remote",
                RequestType.Permission => "Permission",
                _ => "Present"
            };
            return detail;
        }

        detail.Status = "Absent";
        return detail;
    }

    private async Task<AttendancePolicy?> ResolvePolicyAsync(Guid employeeId)
    {
        Employee? employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);

        Guid? departmentId = employee?.EmployeeDepartments
            .FirstOrDefault(ed => ed.EndDate == null)?.DepartmentId;

        if (departmentId is null) return null;

        Department? department = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentId.Value);
        if (department?.PolicyId is null) return null;

        return await _unitOfWork.AttendancePolicyRepository.GetByIdAsync(department.PolicyId.Value);
    }
}
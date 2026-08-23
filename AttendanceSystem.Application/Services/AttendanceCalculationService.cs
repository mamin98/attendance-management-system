using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class AttendanceCalculationService(IUnitOfWork unitOfWork) : IAttendanceCalculationService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<MonthlyAttendanceReportDto>> GenerateMonthlyReportAsync(GenerateReportRequestDto request)
    {
        DateTime monthStart = new(request.Year, request.Month, 1);
        DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

        List<Employee> employees = await ResolveTargetEmployeesAsync(request);
        if (employees.Count == 0)
            return [];

        List<Guid> employeeIds = [.. employees.Select(e => e.Id)];

        List<AttendanceLog> logs = [.. await _unitOfWork.AttendanceLogRepository
        .GetByEmployeesAndMonthAsync(employeeIds, request.Year, request.Month)];

        List<LeaveRequest> leaves = [.. await _unitOfWork.LeaveRequestRepository
        .GetApprovedRequestsAsync(employeeIds, monthStart, monthEnd)];

        List<AttendanceRequest> attendanceRequests = [.. await _unitOfWork.AttendanceRequestRepository
        .GetApprovedRequestsAsync(employeeIds, monthStart, monthEnd)];

        List<EmployeeShift> shiftAssignments = [.. await _unitOfWork.EmployeeShiftRepository
        .GetAllWithSearchAsync(x => x.EmployeeId.HasValue && employeeIds.Contains(x.EmployeeId.Value))];

        List<Guid> shiftIds = [.. shiftAssignments
        .Where(x => x.ShiftId.HasValue)
        .Select(x => x.ShiftId!.Value)
        .Distinct()];

        Dictionary<Guid, Shift> shiftsById = shiftIds.Count == 0
            ? []
            : (await _unitOfWork.ShiftRepository.GetAllWithSearchAsync(s => shiftIds.Contains(s.Id)))
                .ToDictionary(s => s.Id);

        Dictionary<Guid, AttendancePolicy?> policyByEmployee = await ResolvePoliciesAsync(employees);

        ILookup<Guid, AttendanceLog> logsByEmployee = logs.ToLookup(x => x.EmployeeId);
        ILookup<Guid, LeaveRequest> leavesByEmployee = leaves.ToLookup(x => x.EmployeeId);
        ILookup<Guid, AttendanceRequest> requestsByEmployee = attendanceRequests.ToLookup(x => x.EmployeeId);
        ILookup<Guid, EmployeeShift> shiftAssignmentsByEmployee = shiftAssignments.ToLookup(x => x.EmployeeId!.Value);

        List<MonthlyAttendanceReportDto> reports = new(employees.Count);

        foreach (Employee employee in employees)
        {
            reports.Add(BuildReportForEmployee(
                employee,
                monthStart,
                monthEnd,
                [.. logsByEmployee[employee.Id]],
                [.. shiftAssignmentsByEmployee[employee.Id]],
                [.. leavesByEmployee[employee.Id]],
                [.. requestsByEmployee[employee.Id]],
                policyByEmployee.GetValueOrDefault(employee.Id),
                shiftsById));
        }

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

        if (request.DepartmentId.HasValue)
        {
            List<Employee> employeesExistsInDepartment = [.. await _unitOfWork.EmployeeRepository.GetAllWithSearchAsync(
            e => e.EmployeeDepartments.Any(ed => ed.DepartmentId == request.DepartmentId && ed.EndDate == null))];

            return [.. employeesExistsInDepartment];
        }

        return [];
    }

    private async Task<Dictionary<Guid, AttendancePolicy?>> ResolvePoliciesAsync(List<Employee> employees)
    {
        Dictionary<Guid, Guid> employeeToDepartment = employees
            .Select(e => new
            {
                EmployeeId = e.Id,
                e.EmployeeDepartments.FirstOrDefault(ed => ed.EndDate is null)?.DepartmentId
            })
            .Where(x => x.DepartmentId.HasValue)
            .ToDictionary(x => x.EmployeeId, x => x.DepartmentId!.Value);

        Dictionary<Guid, AttendancePolicy?> result = employees.ToDictionary(e => e.Id, _ => (AttendancePolicy?)null);

        if (!employeeToDepartment.Any())
            return result;

        List<Guid> departmentIds = [.. employeeToDepartment.Values.Distinct()];

        List<Department> departments = [.. await _unitOfWork.DepartmentRepository
            .GetAllWithSearchAsync(d => departmentIds.Contains(d.Id))];

        List<Guid> policyIds = [.. departments
            .Where(d => d.PolicyId.HasValue)
            .Select(d => d.PolicyId!.Value)
            .Distinct()];

        Dictionary<Guid, AttendancePolicy> policiesById = policyIds.Count == 0
            ? []
            : (await _unitOfWork.AttendancePolicyRepository.GetAllWithSearchAsync(p => policyIds.Contains(p.Id)))
                .ToDictionary(p => p.Id);

        Dictionary<Guid, Guid?> departmentToPolicyId = departments.ToDictionary(d => d.Id, d => d.PolicyId);

        foreach (KeyValuePair<Guid, Guid> pair in employeeToDepartment)
        {
            if (departmentToPolicyId.TryGetValue(pair.Value, out Guid? policyId) 
                && policyId.HasValue
                && policiesById.TryGetValue(policyId.Value, out AttendancePolicy? policy))            
                    result[pair.Key] = policy;            
        }

        return result;
    }

    private static MonthlyAttendanceReportDto BuildReportForEmployee(
        Employee employee,
        DateTime monthStart,
        DateTime monthEnd,
        IReadOnlyList<AttendanceLog> logs,
        IReadOnlyList<EmployeeShift> shiftAssignments,
        IReadOnlyList<LeaveRequest> approvedLeaves,
        IReadOnlyList<AttendanceRequest> approvedRequests,
        AttendancePolicy? policy,
        IReadOnlyDictionary<Guid, Shift> shiftsById)
    {
        MonthlyAttendanceReportDto report = new()
        {
            EmployeeData = employee.ToSimpleDto(),
            Month = monthStart.Month,
            Year = monthStart.Year
        };

        for (DateTime date = monthStart; date <= monthEnd; date = date.AddDays(1))
        {
            DailyAttendanceDetailDto detail = BuildDailyDetail(
                date, logs, shiftAssignments, approvedLeaves, approvedRequests, shiftsById);

            report.Days.Add(detail);

            if (!detail.IsWorkingDay)
                continue;

            report.WorkingDaysInMonth++;

            switch (detail.Status)
            {
                case nameof(AttendanceStatus.Present):
                    report.PresentDays++;
                    break;
                case "Absent":
                    report.AbsentDays++;
                    break;
                case "OnLeave":
                    report.ApprovedLeaveDays++;
                    break;
                case "Remote":
                    report.ApprovedRemoteDays++;
                    break;
                case "Permission":
                    report.ApprovedPermissionDays++;
                    break;
                    // "Incomplete" (checked in, no checkout) intentionally isn't counted
                    // as Present/Absent — surface it in Days for the UI to flag.
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

    private static DailyAttendanceDetailDto BuildDailyDetail(
        DateTime date,
        IReadOnlyList<AttendanceLog> logs,
        IReadOnlyList<EmployeeShift> shiftAssignments,
        IReadOnlyList<LeaveRequest> approvedLeaves,
        IReadOnlyList<AttendanceRequest> approvedRequests,
        IReadOnlyDictionary<Guid, Shift> shiftsById)
    {
        DailyAttendanceDetailDto detail = new() { Date = date };

        EmployeeShift? activeAssignment = shiftAssignments.FirstOrDefault(
            x => x.StartDate <= date && (x.EndDate is null || x.EndDate >= date));

        if (activeAssignment?.ShiftId is null ||
            !shiftsById.TryGetValue(activeAssignment.ShiftId.Value, out Shift? shift))
        {
            detail.IsWorkingDay = false;
            detail.Status = nameof(AttendanceStatus.NoShiftAssigned);
            return detail;
        }

        ShiftDay? shiftDay = shift.ShiftDays.FirstOrDefault(x => x.Day == DateOnly.FromDateTime(date));

        if (shiftDay is null || shiftDay.IsOffDay)
        {
            detail.IsWorkingDay = false;
            detail.Status = nameof(AttendanceStatus.OffDay);
            return detail;
        }

        detail.IsWorkingDay = true;

        TimeOnly? expectedStart = shiftDay.ShiftDayDetails?.OrderBy(x => x.From).FirstOrDefault()?.From;
        TimeOnly? expectedEnd = shiftDay.ShiftDayDetails?.OrderByDescending(x => x.To).FirstOrDefault()?.To;

        detail.ExpectedStart = expectedStart?.ToString("HH:mm");
        detail.ExpectedEnd = expectedEnd?.ToString("HH:mm");

        AttendanceLog? log = logs.FirstOrDefault(x => x.Date.Date == date.Date);

        if (log?.CheckIn is not null)
        {
            detail.ActualCheckIn = log.CheckIn.Value.ToString("HH:mm");
            detail.ActualCheckOut = log.CheckOut?.ToString("HH:mm");

            if (expectedStart.HasValue)
            {
                TimeSpan checkInValue = log.CheckIn.Value.ToTimeSpan();
                TimeSpan latestAllowed = expectedStart.Value.ToTimeSpan()
                    .Add(TimeSpan.FromMinutes(shift.GracePeriodMinutes));

                if (checkInValue > latestAllowed)
                    detail.LateMinutes = (int)(checkInValue - latestAllowed).TotalMinutes;
            }

            if (log.CheckOut.HasValue)
            {
                TimeSpan checkOutValue = log.CheckOut.Value.ToTimeSpan();

                if (expectedEnd.HasValue && checkOutValue < expectedEnd.Value.ToTimeSpan())
                    detail.EarlyLeaveMinutes = (int)(expectedEnd.Value.ToTimeSpan() - checkOutValue).TotalMinutes;

                detail.WorkedHours = (decimal)(log.CheckOut.Value - log.CheckIn.Value).TotalHours;
                detail.Status = nameof(AttendanceStatus.Present);
            }
            else
            {
                // Checked in but never checked out — don't silently treat as Present,
                // and don't crash on the null CheckOut like the original code did.
                detail.Status = "Incomplete";
            }

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
                _ => nameof(AttendanceStatus.Present)
            };

            return detail;
        }

        detail.Status = "Absent";
        return detail;
    }
}
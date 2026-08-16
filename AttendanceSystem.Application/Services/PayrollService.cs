using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class PayrollService(
    IUnitOfWork unitOfWork,
    IAttendanceCalculationService attendanceCalculationService) : IPayrollService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;
    readonly IAttendanceCalculationService _attendanceCalculationService = attendanceCalculationService;

    public async Task<List<PayrollRecordDto>> GeneratePayrollAsync(GenerateReportRequestDto request)
    {
        List<MonthlyAttendanceReportDto> attendanceReports =
            await _attendanceCalculationService.GenerateMonthlyReportAsync(request);

        List<PayrollRecordDto> results = [];

        foreach (MonthlyAttendanceReportDto report in attendanceReports)
        {
            Guid employeeId = report.EmployeeData?.Id ?? Guid.Empty;            

            SalaryStructure? salary = await _unitOfWork.SalaryStructureRepository
                .GetByEmployeeIdAsync(employeeId);

            if (salary is null)
                continue; // no salary configured -> skip, don't fabricate a payslip

            decimal lateDeduction = report.TotalLateMinutes * salary.LateDeductionPerMinute;
            decimal absenceDeduction = report.AbsentDays * salary.AbsenceDeductionPerDay;

            PayrollRecord? existing = await _unitOfWork.PayrollRecordRepository
                .GetAsync(employeeId, request.Year, request.Month);

            if (existing is not null)
                _unitOfWork.PayrollRecordRepository.Delete(existing); // regenerate: soft-delete stale, create fresh

            PayrollRecord record = PayrollRecord.Create(
                employeeId, request.Month, request.Year,
                salary.BaseSalary, lateDeduction, absenceDeduction);

            await _unitOfWork.PayrollRecordRepository.AddAsync(record);

            results.Add(record.ToDto());
        }

        await _unitOfWork.SaveChangesAsync();
        return results;
    }

    public async Task<List<PayrollRecordDto>> GetEmployeeHistoryAsync(Guid employeeId)
    {
        IReadOnlyList<PayrollRecord> records = await _unitOfWork.PayrollRecordRepository
            .GetByEmployeeIdAsync(employeeId);

        return [.. records.Select(x => new PayrollRecordDto
        {
            Id = x.Id,
            EmployeeData = x.Employee?.ToSimpleDto(),
            Month = x.Month,
            Year = x.Year,
            BaseSalary = x.BaseSalary,
            LateDeductionAmount = x.LateDeductionAmount,
            AbsenceDeductionAmount = x.AbsenceDeductionAmount,
            TotalDeductions = x.TotalDeductions,
            NetSalary = x.NetSalary
        })];
    }
}
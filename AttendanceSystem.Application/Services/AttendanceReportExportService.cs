using AttendanceSystem.Application;
using ClosedXML.Excel;

namespace AttendanceSystem.Infrastructure;

public class AttendanceReportExportService : IAttendanceReportExportService
{
    public Task<byte[]> ExportToExcelAsync(List<MonthlyAttendanceReportDto> reports)
    {
        using XLWorkbook workbook = new();
        IXLWorksheet sheet = workbook.Worksheets.Add("Attendance Report");

        string[] headers =
        [
            "Employee (EN)", "Employee (AR)", "Working Days", "Present Days", "Absent Days",
            "Approved Leave", "Remote Days", "Permission Days", "Worked Hours",
            "Late Days", "Late Minutes", "Early Leave Days", "Early Leave Minutes",
            "Late Policy Exceeded", "Early Leave Policy Exceeded"
        ];

        for (int i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).Value = headers[i];

        int rowIndex = 2;
        foreach (MonthlyAttendanceReportDto r in reports)
        {
            sheet.Cell(rowIndex, 1).Value = r.EmployeeData.NameEnglish;
            sheet.Cell(rowIndex, 2).Value = r.EmployeeData.NameArabic;
            sheet.Cell(rowIndex, 3).Value = r.WorkingDaysInMonth;
            sheet.Cell(rowIndex, 4).Value = r.PresentDays;
            sheet.Cell(rowIndex, 5).Value = r.AbsentDays;
            sheet.Cell(rowIndex, 6).Value = r.ApprovedLeaveDays;
            sheet.Cell(rowIndex, 7).Value = r.ApprovedRemoteDays;
            sheet.Cell(rowIndex, 8).Value = r.ApprovedPermissionDays;
            sheet.Cell(rowIndex, 9).Value = r.TotalWorkedHours;
            sheet.Cell(rowIndex, 10).Value = r.LateDaysCount;
            sheet.Cell(rowIndex, 11).Value = r.TotalLateMinutes;
            sheet.Cell(rowIndex, 12).Value = r.EarlyLeaveDaysCount;
            sheet.Cell(rowIndex, 13).Value = r.TotalEarlyLeaveMinutes;
            sheet.Cell(rowIndex, 14).Value = r.LatePolicyExceeded ? "Yes" : "No";
            sheet.Cell(rowIndex, 15).Value = r.EarlyLeavePolicyExceeded ? "Yes" : "No";
            rowIndex++;
        }

        sheet.Columns().AdjustToContents();

        using MemoryStream stream = new();
        workbook.SaveAs(stream);
        return Task.FromResult(stream.ToArray());
    }
}
using AttendanceSystem.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.API;

[Authorize(Roles = "Admin, Manager")]
[ApiController]
[Route("api/[controller]")]
public class AttendanceImportController(
    IAttendanceImportService importService,
    IAttendanceCalculationService calculationService,
    IAttendanceReportExportService exportService) : ControllerBase
{
    readonly IAttendanceImportService _importService = importService;
    readonly IAttendanceCalculationService _calculationService = calculationService;
    readonly IAttendanceReportExportService _exportService = exportService;

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        ImportAttendanceResultDto result = await _importService.ImportFromExcelAsync(file);

        return Ok(ApiResponse<ImportAttendanceResultDto>.SuccessResponse(
            result,
            $"Processed {result.TotalRows} rows: {result.SuccessCount} succeeded, {result.FailedCount} failed"));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] GenerateReportRequestDto request)
    {
        List<MonthlyAttendanceReportDto> result = await _calculationService.GenerateMonthlyReportAsync(request);

        if (result.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("No employees found for this report"));

        return Ok(ApiResponse<List<MonthlyAttendanceReportDto>>.SuccessResponse(result));
    }

    [HttpGet("report/export")]
    public async Task<IActionResult> ExportReport([FromQuery] GenerateReportRequestDto request)
    {
        List<MonthlyAttendanceReportDto> reports = await _calculationService.GenerateMonthlyReportAsync(request);
        
        if (reports.Count == 0)
            return NotFound(ApiResponse<string>.FailureResponse("There is a problem during export this report"));
        
        byte[] fileBytes = await _exportService.ExportToExcelAsync(reports);

        string fileName = $"AttendanceReport_{request.Year}-{request.Month:D2}.xlsx";

        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
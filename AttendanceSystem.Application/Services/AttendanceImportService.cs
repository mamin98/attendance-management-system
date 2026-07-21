using AttendanceSystem.Application;
using AttendanceSystem.Domain;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace AttendanceSystem.Infrastructure;

public class AttendanceImportService(IUnitOfWork unitOfWork) : IAttendanceImportService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ImportAttendanceResultDto> ImportFromExcelAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
            throw new ValidationException("No file uploaded");

        if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
            throw new ValidationException("Only .xlsx files are supported");

        ImportAttendanceResultDto result = new();
        List<AttendanceLog> toAdd = [];
        List<AttendanceLog> toUpdate = [];

        using Stream stream = file.OpenReadStream();
        using XLWorkbook workbook = new(stream);
        IXLWorksheet worksheet = workbook.Worksheet(1);

        IXLRow? lastRow = worksheet.LastRowUsed()
            ?? throw new ValidationException("The uploaded sheet is empty");

        for (int rowNumber = 2; rowNumber <= lastRow.RowNumber(); rowNumber++)
        {
            IXLRow row = worksheet.Row(rowNumber);

            string email = row.Cell(1).GetString().Trim();
            string dateText = row.Cell(2).GetString().Trim();
            string checkInText = row.Cell(3).GetString().Trim();
            string checkOutText = row.Cell(4).GetString().Trim();

            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(dateText))
                continue;

            result.TotalRows++;

            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new Exception("Employee email is required");

                Employee? employee = await _unitOfWork.EmployeeRepository.GetByEmailAsync(email)
                    ?? throw new Exception($"Employee with email '{email}' not found");

                if (string.IsNullOrWhiteSpace(dateText))
                    throw new Exception("Date is required");

                DateTime date = DateTime.ParseExact(
                    dateText, AttendanceSystemConsts.DateFormat, CultureInfo.InvariantCulture);

                TimeSpan? checkIn = string.IsNullOrWhiteSpace(checkInText) ? null : TimeSpan.Parse(checkInText, CultureInfo.InvariantCulture);
                TimeSpan? checkOut = string.IsNullOrWhiteSpace(checkOutText) ? null : TimeSpan.Parse(checkOutText, CultureInfo.InvariantCulture);

                AttendanceLog? existing = await _unitOfWork.AttendanceLogRepository
                    .GetByEmployeeAndDateAsync(employee.Id, date);

                if (existing is not null)
                {
                    existing.Update(checkIn.ToString(), checkOut.ToString());
                    toUpdate.Add(existing);
                }
                else
                    toAdd.Add(AttendanceLog.Create(employee.Id, date.ToString(), checkIn.ToString(), checkOut.ToString()));

                result.SuccessCount++;
            }
            catch (Exception ex)
            {
                result.FailedCount++;
                result.Errors.Add(new ImportAttendanceRowResultDto
                {
                    RowNumber = rowNumber,
                    EmployeeEmail = email,
                    Error = ex.Message
                });
            }
        }

        if (toAdd.Count > 0)
            await _unitOfWork.AttendanceLogRepository.AddRangeAsync(toAdd);

        foreach (AttendanceLog log in toUpdate)
            _unitOfWork.AttendanceLogRepository.Update(log);

        if (toAdd.Count > 0 || toUpdate.Count > 0)
            await _unitOfWork.SaveChangesAsync();

        return result;
    }
}
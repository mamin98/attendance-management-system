using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class SalaryStructureService(IUnitOfWork unitOfWork) : ISalaryStructureService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<SalaryStructureDto?> GetByEmployeeIdAsync(Guid employeeId)
    {
        SalaryStructure? entity = await _unitOfWork.SalaryStructureRepository.GetByEmployeeIdAsync(employeeId);

        return entity?.ToDto();
    }

    public async Task SetAsync(CreateSalaryStructureDto dto)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        SalaryStructure? existing = await _unitOfWork.SalaryStructureRepository.GetByEmployeeIdAsync(dto.EmployeeId);

        if (existing is not null)
        {
            existing.Update(dto.BaseSalary, dto.LateDeductionPerMinute, dto.AbsenceDeductionPerDay);
            _unitOfWork.SalaryStructureRepository.Update(existing);
        }
        else
        {
            SalaryStructure entity = SalaryStructure.Create(
                dto.EmployeeId, dto.BaseSalary, dto.LateDeductionPerMinute, dto.AbsenceDeductionPerDay);

            await _unitOfWork.SalaryStructureRepository.AddAsync(entity);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
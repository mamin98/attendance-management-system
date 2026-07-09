using AttendanceSystem.Domain;

namespace AttendanceSystem.Application;

public class LeaveTypeService(IUnitOfWork unitOfWork) : ILeaveTypeService
{
    readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<LeaveTypeDto>> GetAllWithPaginationAsync(LeaveTypeSearchDto search)
    {
        PagedResult<LeaveType> data = await _unitOfWork.LeaveTypeRepository.GetAllWithPaginationAsync(search);
        return data.ToPagedDto(x => x.ToDto());
    }

    public async Task<List<LeaveTypeDto>> GetAllAsync()
    {
        IReadOnlyList<LeaveType> data = await _unitOfWork.LeaveTypeRepository.GetAllAsync();
        return [.. data.Select(x => x.ToDto())];
    }

    public async Task<LeaveTypeDto?> GetByIdAsync(Guid id)
    {
        LeaveType? entity = await _unitOfWork.LeaveTypeRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task CreateAsync(CreateLeaveTypeDto dto)
    {
        LeaveType entity = dto.ToEntity();
        await _unitOfWork.LeaveTypeRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, UpdateLeaveTypeDto dto)
    {
        LeaveType? entity = await _unitOfWork.LeaveTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave type not found");

        dto.UpdateEntity(entity);

        _unitOfWork.LeaveTypeRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        LeaveType? entity = await _unitOfWork.LeaveTypeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Leave type not found");

        _unitOfWork.LeaveTypeRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<EmployeeLeaveBalanceDto>> GetEmployeeBalancesAsync(Guid employeeId, int? year = null)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(employeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        IReadOnlyList<EmployeeLeaveBalance> data = await _unitOfWork.EmployeeLeaveBalanceRepository
            .GetByEmployeeIdAsync(employeeId, year);

        return [.. data.Select(x => x.ToDto())];
    }

    public async Task AllocateBalanceAsync(AllocateLeaveBalanceDto dto)
    {
        bool employeeExists = await _unitOfWork.EmployeeRepository.IsExistAsync(dto.EmployeeId);
        if (!employeeExists)
            throw new NotFoundException("Employee not found");

        bool leaveTypeExists = await _unitOfWork.LeaveTypeRepository.IsExistAsync(dto.LeaveTypeId);
        if (!leaveTypeExists)
            throw new NotFoundException("Leave type not found");

        EmployeeLeaveBalance? existing = await _unitOfWork.EmployeeLeaveBalanceRepository
            .GetAsync(dto.EmployeeId, dto.LeaveTypeId, dto.Year);

        if (existing is not null)
        {
            existing.AdjustAllocation(dto.AllocatedDays);
            _unitOfWork.EmployeeLeaveBalanceRepository.Update(existing);
        }
        else
        {
            EmployeeLeaveBalance balance = EmployeeLeaveBalance.Create(
                dto.EmployeeId, dto.LeaveTypeId, dto.Year, dto.AllocatedDays);

            await _unitOfWork.EmployeeLeaveBalanceRepository.AddAsync(balance);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
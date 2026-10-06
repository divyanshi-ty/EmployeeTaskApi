using EmployeeTaskApi.DTOs;

namespace EmployeeTaskApi.Services;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeDto>> GetAllAsync(string? search, string? department, int page, int pageSize);
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<(EmployeeDto? Result, string? Error)> CreateAsync(EmployeeCreateDto dto);
    Task<(bool Found, string? Error)> UpdateAsync(int id, EmployeeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

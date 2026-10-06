using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Models;
using EmployeeTaskApi.Repositories;

namespace EmployeeTaskApi.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repo;
    public EmployeeService(IEmployeeRepository repo) => _repo = repo;

    public async Task<PagedResult<EmployeeDto>> GetAllAsync(string? search, string? department, int page, int pageSize)
    {
        var (items, total) = await _repo.GetPagedAsync(search, department, page, pageSize);
        return new PagedResult<EmployeeDto>(items.Select(ToDto), page, pageSize, total);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        return e is null ? null : ToDto(e);
    }

    public async Task<(EmployeeDto? Result, string? Error)> CreateAsync(EmployeeCreateDto dto)
    {
        if (await _repo.EmailExistsAsync(dto.Email)) return (null, "Email already exists.");

        var e = new Employee
        {
            FullName = dto.FullName, Email = dto.Email,
            Department = dto.Department, Designation = dto.Designation
        };
        await _repo.AddAsync(e);
        return (ToDto(e), null);
    }

    public async Task<(bool Found, string? Error)> UpdateAsync(int id, EmployeeUpdateDto dto)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e is null) return (false, null);
        if (await _repo.EmailExistsAsync(dto.Email, id)) return (true, "Email already exists.");

        e.FullName = dto.FullName; e.Email = dto.Email;
        e.Department = dto.Department; e.Designation = dto.Designation;
        await _repo.UpdateAsync(e);
        return (true, null);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e is null) return false;
        await _repo.DeleteAsync(e);
        return true;
    }

    private static EmployeeDto ToDto(Employee e) =>
        new(e.Id, e.FullName, e.Email, e.Department, e.Designation, e.CreatedAt);
}

using EmployeeTaskApi.Models;

namespace EmployeeTaskApi.Repositories;

public interface IEmployeeRepository
{
    Task<(List<Employee> Items, int Total)> GetPagedAsync(string? search, string? department, int page, int pageSize);
    Task<Employee?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
}

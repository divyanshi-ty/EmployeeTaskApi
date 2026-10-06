using EmployeeTaskApi.Data;
using EmployeeTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _db;
    public EmployeeRepository(AppDbContext db) => _db = db;

    public async Task<(List<Employee> Items, int Total)> GetPagedAsync(string? search, string? department, int page, int pageSize)
    {
        var q = _db.Employees.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(e => e.FullName.Contains(search) || e.Email.Contains(search) || e.Designation.Contains(search));
        if (!string.IsNullOrWhiteSpace(department))
            q = q.Where(e => e.Department == department);

        var total = await q.CountAsync();
        var items = await q.OrderBy(e => e.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public Task<Employee?> GetByIdAsync(int id) => _db.Employees.FirstOrDefaultAsync(e => e.Id == id);

    public Task<bool> ExistsAsync(int id) => _db.Employees.AnyAsync(e => e.Id == id);

    public Task<bool> EmailExistsAsync(string email, int? excludeId = null) =>
        _db.Employees.AnyAsync(e => e.Email == email && (excludeId == null || e.Id != excludeId));

    public async Task AddAsync(Employee employee) { _db.Employees.Add(employee); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Employee employee) { _db.Employees.Update(employee); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Employee employee) { _db.Employees.Remove(employee); await _db.SaveChangesAsync(); }
}

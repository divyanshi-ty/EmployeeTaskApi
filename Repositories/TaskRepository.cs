using EmployeeTaskApi.Data;
using EmployeeTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db) => _db = db;

    public async Task<(List<TaskItem> Items, int Total)> GetPagedAsync(string? search, TaskItemStatus? status,
        TaskPriority? priority, int? employeeId, int page, int pageSize)
    {
        var q = _db.Tasks.AsNoTracking().Include(t => t.Employee).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(t => t.Title.Contains(search) || (t.Description != null && t.Description.Contains(search)));
        if (status.HasValue) q = q.Where(t => t.Status == status);
        if (priority.HasValue) q = q.Where(t => t.Priority == priority);
        if (employeeId.HasValue) q = q.Where(t => t.EmployeeId == employeeId);

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public Task<TaskItem?> GetByIdAsync(int id) =>
        _db.Tasks.Include(t => t.Employee).FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(TaskItem task) { _db.Tasks.Add(task); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(TaskItem task) { _db.Tasks.Update(task); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(TaskItem task) { _db.Tasks.Remove(task); await _db.SaveChangesAsync(); }
}

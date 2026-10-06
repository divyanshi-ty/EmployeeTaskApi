using EmployeeTaskApi.Models;

namespace EmployeeTaskApi.Repositories;

public interface ITaskRepository
{
    Task<(List<TaskItem> Items, int Total)> GetPagedAsync(string? search, TaskItemStatus? status,
        TaskPriority? priority, int? employeeId, int page, int pageSize);
    Task<TaskItem?> GetByIdAsync(int id);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}

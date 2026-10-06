using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Models;
using EmployeeTaskApi.Repositories;

namespace EmployeeTaskApi.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IEmployeeRepository _employees;

    public TaskService(ITaskRepository tasks, IEmployeeRepository employees)
    {
        _tasks = tasks; _employees = employees;
    }

    public async Task<PagedResult<TaskDto>> GetAllAsync(string? search, TaskItemStatus? status, TaskPriority? priority,
        int? employeeId, int page, int pageSize)
    {
        var (items, total) = await _tasks.GetPagedAsync(search, status, priority, employeeId, page, pageSize);
        return new PagedResult<TaskDto>(items.Select(ToDto), page, pageSize, total);
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var t = await _tasks.GetByIdAsync(id);
        return t is null ? null : ToDto(t);
    }

    public async Task<(TaskDto? Result, string? Error)> CreateAsync(TaskCreateDto dto)
    {
        if (!await _employees.ExistsAsync(dto.EmployeeId)) return (null, "Employee not found.");

        var t = new TaskItem
        {
            Title = dto.Title, Description = dto.Description, Priority = dto.Priority,
            DueDate = dto.DueDate, EmployeeId = dto.EmployeeId
        };
        await _tasks.AddAsync(t);
        var saved = await _tasks.GetByIdAsync(t.Id);
        return (ToDto(saved!), null);
    }

    public async Task<(bool Found, string? Error)> UpdateAsync(int id, TaskUpdateDto dto)
    {
        var t = await _tasks.GetByIdAsync(id);
        if (t is null) return (false, null);
        if (!await _employees.ExistsAsync(dto.EmployeeId)) return (true, "Employee not found.");

        t.Title = dto.Title; t.Description = dto.Description; t.Status = dto.Status;
        t.Priority = dto.Priority; t.DueDate = dto.DueDate; t.EmployeeId = dto.EmployeeId;
        await _tasks.UpdateAsync(t);
        return (true, null);
    }

    public async Task<bool> UpdateStatusAsync(int id, TaskItemStatus status)
    {
        var t = await _tasks.GetByIdAsync(id);
        if (t is null) return false;
        t.Status = status;
        await _tasks.UpdateAsync(t);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var t = await _tasks.GetByIdAsync(id);
        if (t is null) return false;
        await _tasks.DeleteAsync(t);
        return true;
    }

    private static TaskDto ToDto(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.Status, t.Priority, t.DueDate, t.CreatedAt, t.EmployeeId, t.Employee?.FullName);
}

using EmployeeTaskApi.DTOs;
using EmployeeTaskApi.Models;

namespace EmployeeTaskApi.Services;

public interface ITaskService
{
    Task<PagedResult<TaskDto>> GetAllAsync(string? search, TaskItemStatus? status, TaskPriority? priority,
        int? employeeId, int page, int pageSize);
    Task<TaskDto?> GetByIdAsync(int id);
    Task<(TaskDto? Result, string? Error)> CreateAsync(TaskCreateDto dto);
    Task<(bool Found, string? Error)> UpdateAsync(int id, TaskUpdateDto dto);
    Task<bool> UpdateStatusAsync(int id, TaskItemStatus status);
    Task<bool> DeleteAsync(int id);
}

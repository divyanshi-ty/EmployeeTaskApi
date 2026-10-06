using System.ComponentModel.DataAnnotations;
using EmployeeTaskApi.Models;

namespace EmployeeTaskApi.DTOs;

public class TaskCreateDto
{
    [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    [Required] public int EmployeeId { get; set; }
}

public class TaskUpdateDto
{
    [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Description { get; set; }
    public TaskItemStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    [Required] public int EmployeeId { get; set; }
}

public class TaskStatusUpdateDto
{
    [Required] public TaskItemStatus Status { get; set; }
}

public record TaskDto(int Id, string Title, string? Description, TaskItemStatus Status, TaskPriority Priority,
    DateTime? DueDate, DateTime CreatedAt, int EmployeeId, string? EmployeeName);

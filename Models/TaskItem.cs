namespace EmployeeTaskApi.Models;

public enum TaskItemStatus { Pending, InProgress, Completed }
public enum TaskPriority { Low, Medium, High }

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}

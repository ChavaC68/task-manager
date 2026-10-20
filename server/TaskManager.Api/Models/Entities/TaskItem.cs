namespace TaskManager.Api.Models.Entities;

/// <summary>ישות המשימה — המודל הפנימי של הדומיין.</summary>
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateOnly DueDate { get; set; }
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
}
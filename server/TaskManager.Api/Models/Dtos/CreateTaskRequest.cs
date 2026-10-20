namespace TaskManager.Api.Models.Dtos;

/// <summary>גוף בקשת POST /api/tasks.</summary>
public record CreateTaskRequest(
    string Title,
    string? Description,
    string Priority,
    string DueDate,
    string Status);
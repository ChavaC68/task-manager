namespace TaskManager.Api.Models.Dtos;

/// <summary>גוף בקשת PUT /api/tasks/{id}.</summary>
public record UpdateTaskRequest(
    string Title,
    string? Description,
    string Priority,
    string DueDate,
    string Status);
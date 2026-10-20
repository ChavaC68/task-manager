namespace TaskManager.Api.Models.Dtos;

public record CreateTaskRequest(
    string Title,
    string? Description,
    string Priority,
    string DueDate,
    string Status);
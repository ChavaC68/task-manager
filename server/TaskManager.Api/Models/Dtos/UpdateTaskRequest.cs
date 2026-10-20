namespace TaskManager.Api.Models.Dtos;

public record UpdateTaskRequest(
    string Title,
    string? Description,
    string Priority,
    string DueDate,
    string Status);
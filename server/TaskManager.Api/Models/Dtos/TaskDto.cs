namespace TaskManager.Api.Models.Dtos;

public record TaskDto(
    int Id,
    string Title,
    string? Description,
    string Priority,
    string DueDate,
    string Status);
namespace TaskManager.Api.Models.Dtos;

/// <summary>מה שהשרת מחזיר ללקוח — תמיד במחרוזות, לפי החוזה.</summary>
public record TaskDto(
    int Id,
    string Title,
    string? Description,
    string Priority,
    string DueDate,
    string Status);
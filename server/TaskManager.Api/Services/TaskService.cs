using System.Globalization;
using TaskManager.Api.Constants;
using TaskManager.Api.Models.Dtos;
using TaskManager.Api.Models.Entities;
using TaskManager.Api.Repositories;

namespace TaskManager.Api.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<TaskDto>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(MapToDto).ToList();
    }

    public async Task<TaskDto> CreateAsync(CreateTaskRequest request)
    {
        var item = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = NormalizeDescription(request.Description),
            Priority = ParsePriority(request.Priority),
            DueDate = ParseDueDate(request.DueDate),
            Status = ParseStatus(request.Status)
        };

        var created = await _repository.AddAsync(item);
        _logger.LogInformation("Task {Id} created with priority {Priority}", created.Id, created.Priority);

        return MapToDto(created);
    }

    public async Task<TaskDto?> UpdateAsync(int id, UpdateTaskRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Title = request.Title.Trim();
        existing.Description = NormalizeDescription(request.Description);
        existing.Priority = ParsePriority(request.Priority);
        existing.DueDate = ParseDueDate(request.DueDate);
        existing.Status = ParseStatus(request.Status);

        var updated = await _repository.UpdateAsync(existing);
        if (updated is null) return null;

        _logger.LogInformation("Task {Id} updated", id);
        return MapToDto(updated);
    }

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);

    private static TaskDto MapToDto(TaskItem item) => new(
        item.Id,
        item.Title,
        item.Description,
        ToHebrewPriority(item.Priority),
        item.DueDate.ToString(TaskConstants.DateFormat, CultureInfo.InvariantCulture),
        ToHebrewStatus(item.Status));

    private static string? NormalizeDescription(string? description) =>
        string.IsNullOrWhiteSpace(description) ? null : description.Trim();

    private static DateOnly ParseDueDate(string value) =>
        DateOnly.ParseExact(value, TaskConstants.DateFormat, CultureInfo.InvariantCulture);

    private static TaskPriority ParsePriority(string value) =>
        TaskConstants.PriorityMap.TryGetValue(value.Trim(), out var result)
            ? result
            : throw new ArgumentException($"עדיפות לא חוקית: {value}", nameof(value));

    private static TaskItemStatus ParseStatus(string value) =>
        TaskConstants.StatusMap.TryGetValue(value.Trim(), out var result)
            ? result
            : throw new ArgumentException($"סטטוס לא חוקי: {value}", nameof(value));

    private static string ToHebrewPriority(TaskPriority priority) =>
        TaskConstants.PriorityMap.First(p => p.Value == priority).Key;

    private static string ToHebrewStatus(TaskItemStatus status) =>
        TaskConstants.StatusMap.First(s => s.Value == status).Key;
}
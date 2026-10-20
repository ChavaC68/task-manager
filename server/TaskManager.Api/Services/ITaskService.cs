using TaskManager.Api.Models.Dtos;

namespace TaskManager.Api.Services;

/// <summary>חוזה ללוגיקה העסקית של משימות.</summary>
public interface ITaskService
{
    Task<IReadOnlyList<TaskDto>> GetAllAsync();
    Task<TaskDto> CreateAsync(CreateTaskRequest request);
    Task<TaskDto?> UpdateAsync(int id, UpdateTaskRequest request);
    Task<bool> DeleteAsync(int id);
}
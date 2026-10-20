using TaskManager.Api.Models.Dtos;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<IReadOnlyList<TaskDto>> GetAllAsync();
    Task<TaskDto> CreateAsync(CreateTaskRequest request);
    Task<TaskDto?> UpdateAsync(int id, UpdateTaskRequest request);
    Task<bool> DeleteAsync(int id);
}
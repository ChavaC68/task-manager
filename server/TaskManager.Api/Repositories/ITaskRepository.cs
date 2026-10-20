using TaskManager.Api.Models.Entities;

namespace TaskManager.Api.Repositories;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem item);
    Task<TaskItem?> UpdateAsync(TaskItem item);
    Task<bool> DeleteAsync(int id);
}
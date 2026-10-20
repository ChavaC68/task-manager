using TaskManager.Api.Models.Entities;

namespace TaskManager.Api.Repositories;

/// <summary>
/// חוזה לגישה לנתונים. המימוש יכול להיות קובץ, SQL, Mongo — ה-Service לא יודע.
/// </summary>
public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem item);
    Task<TaskItem?> UpdateAsync(TaskItem item);
    Task<bool> DeleteAsync(int id);
}
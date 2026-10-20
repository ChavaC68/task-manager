using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManager.Api.Models.Entities;

namespace TaskManager.Api.Repositories;

/// <summary>
/// מימוש ITaskRepository על גבי קובץ JSON.
/// הנתונים נטענים לקריאה, כל שינוי נכתב חזרה לקובץ.
/// </summary>
public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly ILogger<JsonTaskRepository> _logger;

    // מונע גישה מקבילה לקובץ — שתי בקשות במקביל לא יכתבו בו-זמנית (corruption).
    private readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,                        // קובץ קריא בעין
        PropertyNameCaseInsensitive = true,          // "Title" או "title" — לא משנה
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // כתיבה camelCase, תואם לחוזה
        Converters = { new JsonStringEnumConverter() }     // enum כמחרוזת, לא כמספר
    };

    public JsonTaskRepository(IWebHostEnvironment env, ILogger<JsonTaskRepository> logger)
    {
        _logger = logger;
        _filePath = Path.Combine(env.ContentRootPath, "Data", "tasks.json");
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            return await ReadAllInternalAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TaskItem> AddAsync(TaskItem item)
    {
        await _lock.WaitAsync();
        try
        {
            var all = (await ReadAllInternalAsync()).ToList();

            item.Id = all.Count == 0 ? 1 : all.Max(t => t.Id) + 1;
            all.Add(item);

            await WriteAllInternalAsync(all);
            return item;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem item)
    {
        await _lock.WaitAsync();
        try
        {
            var all = (await ReadAllInternalAsync()).ToList();
            var index = all.FindIndex(t => t.Id == item.Id);
            if (index == -1)
            {
                _logger.LogWarning("Task {Id} not found for update", item.Id);
                return null;
            }

            all[index] = item;
            await WriteAllInternalAsync(all);
            return item;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _lock.WaitAsync();
        try
        {
            var all = (await ReadAllInternalAsync()).ToList();
            var removed = all.RemoveAll(t => t.Id == id);
            if (removed == 0)
            {
                _logger.LogWarning("Task {Id} not found for delete", id);
                return false;
            }

            await WriteAllInternalAsync(all);
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }

    // ---- עזר פרטי ----

    private async Task<List<TaskItem>> ReadAllInternalAsync()
    {
        if (!File.Exists(_filePath))
        {
            _logger.LogWarning("Tasks file not found at {Path}. Starting with an empty list.", _filePath);
            return new List<TaskItem>();
        }

        await using var stream = File.OpenRead(_filePath);
        var list = await JsonSerializer.DeserializeAsync<List<TaskItem>>(stream, JsonOptions);
        return list ?? new List<TaskItem>();
    }

    private async Task WriteAllInternalAsync(List<TaskItem> items)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, items, JsonOptions);
    }
}
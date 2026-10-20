using TaskManager.Api.Models.Entities;

namespace TaskManager.Api.Constants;

public static class TaskConstants
{
    public const string DateFormat = "yyyy-MM-dd";
    public const int TitleMaxLength = 200;
    public const int DescriptionMaxLength = 2000;

    public static readonly IReadOnlyDictionary<string, TaskPriority> PriorityMap = new Dictionary<string, TaskPriority>
    {
        ["נמוכה"] = TaskPriority.Low,
        ["בינונית"] = TaskPriority.Medium,
        ["גבוהה"] = TaskPriority.High
    };

    public static readonly IReadOnlyDictionary<string, TaskItemStatus> StatusMap = new Dictionary<string, TaskItemStatus>
    {
        ["ממתינה"] = TaskItemStatus.Pending,
        ["בתהליך"] = TaskItemStatus.InProgress,
        ["הושלמה"] = TaskItemStatus.Completed
    };

    public static readonly IReadOnlyList<string> AllowedPriorities = PriorityMap.Keys.ToList();
    public static readonly IReadOnlyList<string> AllowedStatuses = StatusMap.Keys.ToList();
}
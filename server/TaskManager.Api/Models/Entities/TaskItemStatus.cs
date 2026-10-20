namespace TaskManager.Api.Models.Entities;

/// <summary>סטטוס המשימה.</summary>
/// <remarks>השם נבחר כ-TaskItemStatus כדי לא להתנגש עם System.Threading.Tasks.TaskStatus.</remarks>
public enum TaskItemStatus
{
    Pending = 1,     // ממתינה
    InProgress = 2,  // בתהליך
    Completed = 3    // הושלמה
}
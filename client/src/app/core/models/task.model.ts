/**
 * טיפוסי המשימה — משקפים בדיוק את החוזה של השרת.
 * Priority/Status הם union של הערכים בעברית, כפי שהשרת מחזיר ומקבל.
 */

export const TASK_PRIORITIES = ['נמוכה', 'בינונית', 'גבוהה'] as const;
export type TaskPriority = typeof TASK_PRIORITIES[number];

export const TASK_STATUSES = ['ממתינה', 'בתהליך', 'הושלמה'] as const;
export type TaskStatus = typeof TASK_STATUSES[number];

/** מה שהשרת מחזיר. */
export interface Task {
  id: number;
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string; // yyyy-MM-dd
  status: TaskStatus;
}

/** גוף בקשת POST /api/tasks. */
export interface CreateTaskRequest {
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string;
  status: TaskStatus;
}

/** גוף בקשת PUT /api/tasks/{id}. */
export type UpdateTaskRequest = CreateTaskRequest;

/** טופס "עריכה" — מחזיק גם את המזהה של המשימה שנערכת (null = יצירה). */
export interface TaskFormValue {
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string;
  status: TaskStatus;
}

/**
 * צבעי badge לפי עדיפות/סטטוס — מיפויים מרכזיים,
 * כדי לא לחזור עליהם בקומפוננטות.
 */
export const PRIORITY_BADGE: Record<TaskPriority, string> = {
  'נמוכה': 'text-bg-secondary',
  'בינונית': 'text-bg-warning',
  'גבוהה': 'text-bg-danger',
};

export const STATUS_BADGE: Record<TaskStatus, string> = {
  'ממתינה': 'text-bg-secondary',
  'בתהליך': 'text-bg-info',
  'הושלמה': 'text-bg-success',
};
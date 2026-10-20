export const TASK_PRIORITIES = ['נמוכה', 'בינונית', 'גבוהה'] as const;
export type TaskPriority = typeof TASK_PRIORITIES[number];

export const TASK_STATUSES = ['ממתינה', 'בתהליך', 'הושלמה'] as const;
export type TaskStatus = typeof TASK_STATUSES[number];

export interface Task {
  id: number;
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string; // yyyy-MM-dd
  status: TaskStatus;
}

export interface CreateTaskRequest {
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string;
  status: TaskStatus;
}

export type UpdateTaskRequest = CreateTaskRequest;

export interface TaskFormValue {
  title: string;
  description: string | null;
  priority: TaskPriority;
  dueDate: string;
  status: TaskStatus;
}

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
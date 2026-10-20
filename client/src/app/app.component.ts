import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';

import { Task } from './core/models/task.model';
import { TaskService } from './core/services/task.service';
import { TaskFormComponent } from './features/tasks/task-form/task-form.component';
import { TaskListComponent } from './features/tasks/task-list/task-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TaskFormComponent, TaskListComponent],
  templateUrl: './app.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit {
  readonly taskService = inject(TaskService);

  /** המשימה שנערכת כרגע — null = מצב יצירה. */
  readonly editingTask = signal<Task | null>(null);

  ngOnInit(): void {
    this.taskService.loadAll();
  }

  /** לחיצה על עיפרון ברשימה — טוען את המשימה לטופס. */
  onEditRequested(task: Task): void {
    this.editingTask.set(task);
  }

  /** הטופס הסתיים (יצירה או עדכון) — חוזרים למצב יצירה. */
  onFormSaved(): void {
    this.editingTask.set(null);
  }

  /** ביטול עריכה — חוזרים למצב יצירה. */
  onEditCancelled(): void {
    this.editingTask.set(null);
  }

  /** מחיקת משימה — אם היא הייתה בעריכה, מבטלים את העריכה. */
  onDeleteRequested(id: number): void {
    this.taskService.delete(id).subscribe({
      next: () => {
        if (this.editingTask()?.id === id) {
          this.editingTask.set(null);
        }
      },
    });
  }
}
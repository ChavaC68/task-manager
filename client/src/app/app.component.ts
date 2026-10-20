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

  readonly editingTask = signal<Task | null>(null);

  ngOnInit(): void {
    this.taskService.loadAll();
  }

  onEditRequested(task: Task): void {
    this.editingTask.set(task);
  }

  onFormSaved(): void {
    this.editingTask.set(null);
  }

  onEditCancelled(): void {
    this.editingTask.set(null);
  }

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
import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

import { Task } from '../../../core/models/task.model';
import { TaskItemComponent } from '../task-item/task-item.component';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [TaskItemComponent],
  templateUrl: './task-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskListComponent {
  readonly tasks = input.required<Task[]>();
  readonly loading = input<boolean>(false);
  readonly error = input<string | null>(null);

  readonly edit = output<Task>();
  readonly remove = output<number>();
}
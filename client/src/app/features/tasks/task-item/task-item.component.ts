import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';

import { PRIORITY_BADGE, STATUS_BADGE, Task } from '../../../core/models/task.model';

/**
 * כרטיס משימה בודד. תצוגה בלבד — מדווח על עריכה/מחיקה להורה.
 */
@Component({
  selector: 'app-task-item',
  standalone: true,
  imports: [],
  templateUrl: './task-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskItemComponent {
  readonly task = input.required<Task>();

  readonly edit = output<Task>();
  readonly remove = output<number>();

  readonly priorityBadgeClass = computed(
    () => `badge rounded-pill ${PRIORITY_BADGE[this.task().priority]}`
  );
  readonly statusBadgeClass = computed(
    () => `badge rounded-pill ${STATUS_BADGE[this.task().status]}`
  );

  /** תצוגת תאריך בפורמט dd/MM/yyyy. */
  readonly formattedDueDate = computed(() => {
    const due = this.task().dueDate;
    if (!due) return '';
    const [y, m, d] = due.split('-');
    return `${d}/${m}/${y}`;
  });

  onEdit(): void {
    this.edit.emit(this.task());
  }

  onDelete(): void {
    this.remove.emit(this.task().id);
  }
}
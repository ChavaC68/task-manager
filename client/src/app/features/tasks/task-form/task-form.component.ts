import {
    ChangeDetectionStrategy,
    Component,
    effect,
    inject,
    input,
    output,
    signal,
} from '@angular/core';
import {
    FormBuilder,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';

import {
    CreateTaskRequest,
    DEFAULT_PRIORITY,
    DEFAULT_STATUS,
    Task,
    TASK_PRIORITIES,
    TASK_STATUSES,
} from '../../../core/models/task.model';
import { TaskService } from '../../../core/services/task.service';

@Component({
    selector: 'app-task-form',
    standalone: true,
    imports: [ReactiveFormsModule],
    templateUrl: './task-form.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TaskFormComponent {
    private readonly fb = inject(FormBuilder);
    private readonly taskService = inject(TaskService);

    readonly editingTask = input<Task | null>(null);

    readonly saved = output<void>();
    readonly cancelled = output<void>();

    readonly priorities = TASK_PRIORITIES;
    readonly statuses = TASK_STATUSES;

    readonly submitting = signal(false);
    readonly serverError = signal<string | null>(null);

    readonly isEditMode = () => this.editingTask() !== null;

    readonly form = this.fb.nonNullable.group({
        title: ['', [Validators.required, Validators.maxLength(200)]],
        description: [''],
        priority: [DEFAULT_PRIORITY as string, [Validators.required]],
        dueDate: ['', [Validators.required]],
        status: [DEFAULT_STATUS as string, [Validators.required]],
    });

    constructor() {
        effect(() => {
            const task = this.editingTask();
            this.serverError.set(null);

            if (task) {
                this.form.setValue({
                    title: task.title,
                    description: task.description ?? '',
                    priority: task.priority,
                    dueDate: task.dueDate,
                    status: task.status,
                });
            } else {
                this.form.reset({
                    title: '',
                    description: '',
                    priority: DEFAULT_PRIORITY,
                    dueDate: '',
                    status: DEFAULT_STATUS,
                });
            }
        });
    }

    get title() { return this.form.controls.title; }
    get description() { return this.form.controls.description; }
    get priority() { return this.form.controls.priority; }
    get dueDate() { return this.form.controls.dueDate; }
    get status() { return this.form.controls.status; }

    showError(controlName: keyof typeof this.form.controls): boolean {
        const control = this.form.controls[controlName];
        return control.invalid && (control.touched || control.dirty);
    }

    onSubmit(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();
            return;
        }

        this.submitting.set(true);
        this.serverError.set(null);

        const raw = this.form.getRawValue();
        const request: CreateTaskRequest = {
            title: raw.title.trim(),
            description: raw.description.trim() || null,
            priority: raw.priority as CreateTaskRequest['priority'],
            dueDate: raw.dueDate,
            status: raw.status as CreateTaskRequest['status'],
        };

        const editing = this.editingTask();
        const operation = editing
            ? this.taskService.update(editing.id, request)
            : this.taskService.create(request);

        operation.subscribe({
            next: () => {
                this.submitting.set(false);
                if (!editing) {
                    this.resetForm();
                }
                this.saved.emit();
            },
            error: () => {
                this.submitting.set(false);
                this.serverError.set(this.taskService.error());
            },
        });
    }

    onCancel(): void {
        this.resetForm();
        this.cancelled.emit();
    }

    private resetForm(): void {
        this.form.reset({
            title: '',
            description: '',
            priority: DEFAULT_PRIORITY,
            dueDate: '',
            status: DEFAULT_STATUS,
        });
        this.serverError.set(null);
    }
}
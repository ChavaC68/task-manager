import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import {
    CreateTaskRequest,
    PENDING_STATUS,
    Task,
    UpdateTaskRequest,
} from '../models/task.model';

@Injectable({ providedIn: 'root' })
export class TaskService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/tasks`;

    private readonly _tasks = signal<Task[]>([]);
    private readonly _loading = signal(false);
    private readonly _error = signal<string | null>(null);
    private readonly _loadError = signal<string | null>(null);

    readonly tasks = this._tasks.asReadonly();
    readonly loading = this._loading.asReadonly();
    readonly error = this._error.asReadonly();
    readonly loadError = this._loadError.asReadonly();

    readonly count = computed(() => this._tasks().length);
    readonly pendingCount = computed(
        () => this._tasks().filter(t => t.status === PENDING_STATUS).length
    );

    loadAll(): void {
        this._loading.set(true);
        this._loadError.set(null);

        this.http.get<Task[]>(this.apiUrl)
            .pipe(
                tap(tasks => this._tasks.set(tasks)),
                catchError(err => this.handleLoadError(err))
            )
            .subscribe({
                next: () => this._loading.set(false),
                error: () => this._loading.set(false),
            });
    }

    create(request: CreateTaskRequest): Observable<Task> {
        return this.http.post<Task>(this.apiUrl, request).pipe(
            tap(created => this._tasks.update(list => [...list, created])),
            catchError(err => this.handleError(err, 'יצירת המשימה נכשלה.'))
        );
    }

    update(id: number, request: UpdateTaskRequest): Observable<Task> {
        return this.http.put<Task>(`${this.apiUrl}/${id}`, request).pipe(
            tap(updated =>
                this._tasks.update(list =>
                    list.map(t => (t.id === updated.id ? updated : t))
                )
            ),
            catchError(err => this.handleError(err, 'עדכון המשימה נכשל.'))
        );
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
            tap(() => this._tasks.update(list => list.filter(t => t.id !== id))),
            catchError(err => this.handleError(err, 'מחיקת המשימה נכשלה.'))
        );
    }

    private handleError(err: HttpErrorResponse, fallback: string): Observable<never> {
        let message = fallback;

        if (err.status === 0) {
            message = 'אין חיבור לשרת. בדקו שהשרת פועל ונסו שוב.';
        } else if (err.error?.errors) {
            const errors = err.error.errors as Record<string, string[]>;
            message = Object.values(errors).flat().join(' ');
        } else if (err.error?.title) {
            message = err.error.title;
        }

        this._error.set(message);
        return throwError(() => err);
    }

    private handleLoadError(err: HttpErrorResponse): Observable<never> {
        this._loadError.set(
            err.status === 0
                ? 'אין חיבור לשרת. בדקו שהשרת פועל ונסו שוב.'
                : 'טעינת המשימות נכשלה.'
        );
        return throwError(() => err);
    }
}
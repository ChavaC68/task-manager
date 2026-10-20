using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models.Dtos;
using TaskManager.Api.Services;

namespace TaskManager.Api.Endpoints;

/// <summary>
/// Endpoints של /tasks. Minimal API — ללא Controllers.
/// תלוי ב-ITaskService ובוולדטורים בלבד.
/// </summary>
public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks")
                       .WithTags("Tasks");

        // GET /tasks
        group.MapGet("/", async (ITaskService service) =>
            {
                var tasks = await service.GetAllAsync();
                return Results.Ok(tasks);
            })
            .Produces<IReadOnlyList<TaskDto>>(StatusCodes.Status200OK)
            .WithName("GetAllTasks");

        // POST /tasks
        group.MapPost("/", async (
                CreateTaskRequest request,
                IValidator<CreateTaskRequest> validator,
                ITaskService service) =>
            {
                var validation = await validator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    return Results.ValidationProblem(validation.ToDictionary());
                }

                var created = await service.CreateAsync(request);
                return Results.Created($"/tasks/{created.Id}", created);
            })
            .Produces<TaskDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithName("CreateTask");

        // PUT /tasks/{id}
        group.MapPut("/{id:int}", async (
                int id,
                UpdateTaskRequest request,
                IValidator<UpdateTaskRequest> validator,
                ITaskService service) =>
            {
                var validation = await validator.ValidateAsync(request);
                if (!validation.IsValid)
                {
                    return Results.ValidationProblem(validation.ToDictionary());
                }

                var updated = await service.UpdateAsync(id, request);
                return updated is null
                    ? Results.NotFound(NotFoundProblem("המשימה לא נמצאה."))
                    : Results.Ok(updated);
            })
            .Produces<TaskDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("UpdateTask");

        // DELETE /tasks/{id}
        group.MapDelete("/{id:int}", async (int id, ITaskService service) =>
            {
                var deleted = await service.DeleteAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound(NotFoundProblem("המשימה לא נמצאה."));
            })
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("DeleteTask");

        return app;
    }

    private static ProblemDetails NotFoundProblem(string title) => new()
    {
        Status = StatusCodes.Status404NotFound,
        Title = title
    };
}
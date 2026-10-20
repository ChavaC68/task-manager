using FluentValidation;
using TaskManager.Api.Endpoints;
using TaskManager.Api.Middleware;
using TaskManager.Api.Repositories;
using TaskManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- שירותים ----------

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Task Manager API",
        Version = "v1",
        Description = "API לניהול משימות — ASP.NET Core 8 Minimal API."
    });
});

// CORS — מאפשר ל-Angular (localhost:4200) לפנות לשרת מהדפדפן
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// FluentValidation — סורק את ה-Assembly ורושם אוטומטית את כל הוולדטורים
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// DI — Repository ו-Service דרך interfaces בלבד
builder.Services.AddSingleton<ITaskRepository, JsonTaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

// ---------- Pipeline ----------

var app = builder.Build();

// ראשון בתור — תופס כל חריגה בהמשך השרשרת
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

app.MapAllEndpoints();

app.Run();
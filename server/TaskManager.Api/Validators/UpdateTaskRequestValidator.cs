using FluentValidation;
using TaskManager.Api.Constants;
using TaskManager.Api.Models.Dtos;

namespace TaskManager.Api.Validators;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("כותרת המשימה היא שדה חובה.")
            .MaximumLength(TaskConstants.TitleMaxLength)
            .WithMessage($"כותרת המשימה לא תעלה על {TaskConstants.TitleMaxLength} תווים.");

        RuleFor(x => x.Description)
            .MaximumLength(TaskConstants.DescriptionMaxLength)
            .WithMessage($"תיאור המשימה לא יעלה על {TaskConstants.DescriptionMaxLength} תווים.");

        RuleFor(x => x.Priority)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("עדיפות המשימה היא שדה חובה.")
            .Must(TaskConstants.AllowedPriorities.Contains).WithMessage("עדיפות המשימה אינה חוקית.");

        RuleFor(x => x.DueDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("תאריך היעד הוא שדה חובה.")
            .Must(BeValidDate).WithMessage($"תאריך היעד אינו בפורמט חוקי ({TaskConstants.DateFormat}).");

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("סטטוס המשימה הוא שדה חובה.")
            .Must(TaskConstants.AllowedStatuses.Contains).WithMessage("סטטוס המשימה אינו חוקי.");
    }

    private static bool BeValidDate(string value) =>
        DateOnly.TryParseExact(value, TaskConstants.DateFormat, out _);
}
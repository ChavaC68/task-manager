using FluentValidation;
using TaskManager.Api.Models.Dtos;

namespace TaskManager.Api.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    private static readonly string[] AllowedPriorities = { "נמוכה", "בינונית", "גבוהה" };
    private static readonly string[] AllowedStatuses = { "ממתינה", "בתהליך", "הושלמה" };
    private const string DateFormat = "yyyy-MM-dd";

    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("כותרת המשימה היא שדה חובה.")
            .MaximumLength(200).WithMessage("כותרת המשימה לא תעלה על 200 תווים.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("תיאור המשימה לא יעלה על 2000 תווים.");

        RuleFor(x => x.Priority)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("עדיפות המשימה היא שדה חובה.")
            .Must(AllowedPriorities.Contains).WithMessage("עדיפות המשימה אינה חוקית.");

        RuleFor(x => x.DueDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("תאריך היעד הוא שדה חובה.")
            .Must(BeValidDate).WithMessage($"תאריך היעד אינו בפורמט חוקי ({DateFormat}).");

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("סטטוס המשימה הוא שדה חובה.")
            .Must(AllowedStatuses.Contains).WithMessage("סטטוס המשימה אינו חוקי.");
    }

    private static bool BeValidDate(string value) =>
        DateOnly.TryParseExact(value, DateFormat, out _);

}
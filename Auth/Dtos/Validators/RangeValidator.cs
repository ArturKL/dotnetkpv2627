using FluentValidation;

namespace Auth.Dtos.Validators;

public class RangeValidator<T> : AbstractValidator<Range<T>>
    where T : struct, IComparable<T>
{
    public RangeValidator()
    {
        RuleFor(x => x)
            .Must(x =>
                !x.From.HasValue ||
                !x.To.HasValue ||
                x.From.Value.CompareTo(x.To.Value) <= 0)
            .WithMessage("'From' must be less than or equal to 'To'.");
    }
}
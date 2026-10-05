using FluentValidation;

namespace Auth.Dtos.Validators;

public class SearchUsersDtoValidator : AbstractValidator<SearchUsersDto>
{
    public SearchUsersDtoValidator()
    {
        RuleFor(x => x.CreatedAtRange).SetValidator(new RangeValidator<DateTime>());
        RuleFor(x => x.UpdatedAtRange).SetValidator(new RangeValidator<DateTime>());
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}
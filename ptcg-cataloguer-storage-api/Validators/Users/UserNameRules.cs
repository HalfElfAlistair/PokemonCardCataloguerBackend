using FluentValidation;
using System.Linq.Expressions;

namespace Cataloguer.Validators;

public abstract class UserNameRules<T> : AbstractValidator<T> where T : class
{
    protected void AddNameRules(Expression<Func<T, string>> selector)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage("A name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
    }
}
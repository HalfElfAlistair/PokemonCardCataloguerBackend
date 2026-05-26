using FluentValidation;
using System.Linq.Expressions;

namespace Cataloguer.Validators;

public abstract class CardCountRules<T> : AbstractValidator<T> where T : class
{
    protected void AddCountRules(Expression<Func<T, int>> selector)
    {
        RuleFor(selector)
            .GreaterThanOrEqualTo(0).WithMessage("Count must not be a negative number.");
    }
}
using FluentValidation;
using Cataloguer.Dtos;

namespace Cataloguer.Validators;

public class ListCreateDtoValidator : ListNameRules<CardsListCreateDto>
{
    public ListCreateDtoValidator()
    {
        AddNameRules(l => l.ListName);
        RuleFor(l => l.CardIDs).Empty().WithMessage("List must be empty when creating this item.");
    }
}
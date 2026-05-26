using FluentValidation;
using Cataloguer.Dtos;

namespace Cataloguer.Validators;

public class ListNameUpdateDtoValidator : ListNameRules<CardsListNameUpdateDto>
{
    public ListNameUpdateDtoValidator()
    {
        AddNameRules(l => l.ListName);
    }
}
using FluentValidation;
using Cataloguer.Dtos;

namespace Cataloguer.Validators;

public class CardUpdateDtoValidator : CardCountRules<CardUpdateDto>
{
    public CardUpdateDtoValidator()
    {
        AddCountRules(u => u.Count);
    }
}
using FluentValidation;
using Cataloguer.Dtos;

namespace Cataloguer.Validators;

public class UserCreateDtoValidator : UserNameRules<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        AddNameRules(u => u.Name);
    }
}
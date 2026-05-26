using FluentValidation;
using Cataloguer.Dtos;

namespace Cataloguer.Validators;

public class UserUpdateDtoValidator : UserNameRules<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        AddNameRules(u => u.Name);
    }
}
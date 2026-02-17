using FluentValidation;
using MinimalApi.DTOs;

namespace MinimalApi.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}


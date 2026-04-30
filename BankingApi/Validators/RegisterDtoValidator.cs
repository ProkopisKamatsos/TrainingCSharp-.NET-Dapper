using BankingApi.DTOs.Auth;
using FluentValidation;

namespace BankingApi.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.PhoneNumber).Matches(@"^\+?[\d\s\-()]{7,20}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}

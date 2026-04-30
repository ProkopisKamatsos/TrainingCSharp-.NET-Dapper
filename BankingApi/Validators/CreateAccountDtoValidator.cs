using BankingApi.DTOs.Account;
using FluentValidation;

namespace BankingApi.Validators;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    private static readonly string[] AllowedTypes = ["Checking", "Savings"];

    public CreateAccountDtoValidator()
    {
        RuleFor(x => x.AccountType)
            .NotEmpty()
            .Must(t => AllowedTypes.Contains(t))
            .WithMessage("AccountType must be 'Checking' or 'Savings'.");
    }
}

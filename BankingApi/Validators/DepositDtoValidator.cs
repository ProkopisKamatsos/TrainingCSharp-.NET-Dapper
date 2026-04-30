using BankingApi.DTOs.Transaction;
using FluentValidation;

namespace BankingApi.Validators;

public class DepositDtoValidator : AbstractValidator<DepositDto>
{
    public DepositDtoValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

using BankingApi.DTOs.Transaction;
using FluentValidation;

namespace BankingApi.Validators;

public class WithdrawDtoValidator : AbstractValidator<WithdrawDto>
{
    public WithdrawDtoValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

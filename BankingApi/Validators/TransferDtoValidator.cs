using BankingApi.DTOs.Transaction;
using FluentValidation;

namespace BankingApi.Validators;

public class TransferDtoValidator : AbstractValidator<TransferDto>
{
    public TransferDtoValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.ToAccountId)
            .NotEqual(x => x.FromAccountId)
            .WithMessage("FromAccountId and ToAccountId must be different.");
    }
}

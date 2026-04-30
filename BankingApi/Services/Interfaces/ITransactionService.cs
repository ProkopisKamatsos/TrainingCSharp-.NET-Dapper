using BankingApi.DTOs.Transaction;

namespace BankingApi.Services.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponseDto> DepositAsync(DepositDto dto, int customerId);
    Task<TransactionResponseDto> WithdrawAsync(WithdrawDto dto, int customerId);
    Task<TransactionResponseDto> TransferAsync(TransferDto dto, int customerId);
    Task<IEnumerable<TransactionResponseDto>> GetByAccountIdAsync(int accountId, int customerId);
}
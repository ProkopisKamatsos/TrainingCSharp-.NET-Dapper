using BankingApi.DTOs.Account;

namespace BankingApi.Services.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<AccountResponseDto>> GetMyAccountsAsync(int customerId);
    Task<AccountResponseDto> GetByIdAsync(int accountId, int customerId);
    Task<AccountResponseDto> CreateAsync(CreateAccountDto dto, int customerId);
    Task DeactivateAsync(int accountId, int customerId);
}
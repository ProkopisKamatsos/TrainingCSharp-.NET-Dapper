using BankingApi.DTOs.Account;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;

namespace BankingApi.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IAccountRepository accountRepository,
        ILogger<AccountService> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<AccountResponseDto>> GetMyAccountsAsync(int customerId)
    {
        var accounts = await _accountRepository.GetByCustomerIdAsync(customerId);
        return accounts.Select(MapToDto);
    }

    public async Task<AccountResponseDto> GetByIdAsync(int accountId, int customerId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);

        if (account == null)
            throw new KeyNotFoundException("Account not found.");

        // Authorization check — το account ανήκει στον συγκεκριμένο customer;
        if (account.CustomerId != customerId)
            throw new UnauthorizedAccessException("Access denied.");

        return MapToDto(account);
    }

    public async Task<AccountResponseDto> CreateAsync(CreateAccountDto dto, int customerId)
    {
        if (dto.AccountType != "Checking" && dto.AccountType != "Savings")
            throw new ArgumentException("Account type must be Checking or Savings.");

        var account = new Account
        {
            CustomerId = customerId,
            AccountNumber = GenerateAccountNumber(),
            AccountType = dto.AccountType,
            Balance = 0
        };

        var accountId = await _accountRepository.CreateAsync(account);

        // Fetch από τη βάση για να πάρουμε τα σωστά timestamps
        var created = await _accountRepository.GetByIdAsync(accountId);

        _logger.LogInformation(
            "Account created: {AccountNumber} for CustomerId: {CustomerId}",
            account.AccountNumber, customerId);

        return MapToDto(created!);
    }

    public async Task DeactivateAsync(int accountId, int customerId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);

        if (account == null)
            throw new KeyNotFoundException("Account not found.");

        // Authorization check
        if (account.CustomerId != customerId)
            throw new UnauthorizedAccessException("Access denied.");

        if (account.Balance > 0)
            throw new InvalidOperationException("Cannot close account with remaining balance.");

        await _accountRepository.DeactivateAsync(accountId);

        _logger.LogInformation(
            "Account deactivated: {AccountId} for CustomerId: {CustomerId}",
            accountId, customerId);
    }

    private static AccountResponseDto MapToDto(Account account) => new()
    {
        AccountId = account.AccountId,
        AccountNumber = account.AccountNumber,
        AccountType = account.AccountType,
        Balance = account.Balance,
        CreatedAt = account.CreatedAt
    };

    private static string GenerateAccountNumber()
    {
        // Απλό format: GR + timestamp + random — αρκετό για development
        return $"GR{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
    }
}
using BankingApi.Data;
using BankingApi.DTOs.Transaction;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;

namespace BankingApi.Services;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly SqlConnectionFactory _connectionFactory;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        SqlConnectionFactory connectionFactory,
        ILogger<TransactionService> logger)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<TransactionResponseDto> DepositAsync(DepositDto dto, int customerId)
    {
        var account = await _accountRepository.GetByIdAsync(dto.ToAccountId);

        if (account == null)
            throw new KeyNotFoundException("Account not found.");

        // Authorization check
        if (account.CustomerId != customerId)
            throw new UnauthorizedAccessException("Access denied.");

        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        using var sqlTransaction = await connection.BeginTransactionAsync() as Microsoft.Data.SqlClient.SqlTransaction;

        try
        {
            // 1. Ενημέρωση balance
            await _accountRepository.UpdateBalanceAsync(dto.ToAccountId, account.Balance + dto.Amount, connection, sqlTransaction!);

            // 2. Καταγραφή transaction
            var transaction = new Transaction
            {
                FromAccountId = null,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                TransactionType = "Deposit",
                Status = "Completed",
                Description = dto.Description
            };

            var transactionId = await _transactionRepository.CreateAsync(transaction, connection, sqlTransaction!);
            transaction.TransactionId = transactionId;

            await sqlTransaction!.CommitAsync();

            _logger.LogInformation(
                "Deposit completed: {Amount} to AccountId {AccountId}",
                dto.Amount, dto.ToAccountId);

            var created = await _transactionRepository.GetByIdAsync(transactionId);
            return MapToDto(created!);
        }
        catch
        {
            await sqlTransaction!.RollbackAsync();
            throw;
        }
    }

    public async Task<TransactionResponseDto> WithdrawAsync(WithdrawDto dto, int customerId)
    {
        var account = await _accountRepository.GetByIdAsync(dto.FromAccountId);

        if (account == null)
            throw new KeyNotFoundException("Account not found.");

        // Authorization check
        if (account.CustomerId != customerId)
            throw new UnauthorizedAccessException("Access denied.");

        if (account.Balance < dto.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        using var sqlTransaction = await connection.BeginTransactionAsync() as Microsoft.Data.SqlClient.SqlTransaction;

        try
        {
            // 1. Ενημέρωση balance
            await _accountRepository.UpdateBalanceAsync(dto.FromAccountId, account.Balance - dto.Amount, connection, sqlTransaction!);

            // 2. Καταγραφή transaction
            var transaction = new Transaction
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = null,
                Amount = dto.Amount,
                TransactionType = "Withdrawal",
                Status = "Completed",
                Description = dto.Description
            };

            var transactionId = await _transactionRepository.CreateAsync(transaction, connection, sqlTransaction!);
            transaction.TransactionId = transactionId;

            await sqlTransaction!.CommitAsync();

            _logger.LogInformation(
                "Withdrawal completed: {Amount} from AccountId {AccountId}",
                dto.Amount, dto.FromAccountId);

            var created = await _transactionRepository.GetByIdAsync(transactionId);
            return MapToDto(created!);
        }
        catch
        {
            await sqlTransaction!.RollbackAsync();
            throw;
        }
    }

    public async Task<TransactionResponseDto> TransferAsync(TransferDto dto, int customerId)
    {
        if (dto.FromAccountId == dto.ToAccountId)
            throw new ArgumentException("Cannot transfer to the same account.");

        var fromAccount = await _accountRepository.GetByIdAsync(dto.FromAccountId);
        var toAccount = await _accountRepository.GetByIdAsync(dto.ToAccountId);

        if (fromAccount == null || toAccount == null)
            throw new KeyNotFoundException("Account not found.");

        // Authorization check — μόνο το FromAccount πρέπει να ανήκει στον customer
        // Το ToAccount μπορεί να είναι οποιουδήποτε
        if (fromAccount.CustomerId != customerId)
            throw new UnauthorizedAccessException("Access denied.");

        if (fromAccount.Balance < dto.Amount)
            throw new InvalidOperationException("Insufficient funds.");

        using var connection = _connectionFactory.Create();
        await connection.OpenAsync();
        using var sqlTransaction = await connection.BeginTransactionAsync() as Microsoft.Data.SqlClient.SqlTransaction;

        try
        {
            // 1. Αφαίρεση από αποστολέα
            await _accountRepository.UpdateBalanceAsync(dto.FromAccountId, fromAccount.Balance - dto.Amount, connection, sqlTransaction!);

            // 2. Πρόσθεση στον παραλήπτη
            await _accountRepository.UpdateBalanceAsync(dto.ToAccountId, toAccount.Balance + dto.Amount, connection, sqlTransaction!);

            // 3. Καταγραφή transaction
            var transaction = new Transaction
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                TransactionType = "Transfer",
                Status = "Completed",
                Description = dto.Description
            };

            var transactionId = await _transactionRepository.CreateAsync(transaction, connection, sqlTransaction!);
            transaction.TransactionId = transactionId;

            await sqlTransaction!.CommitAsync();

            _logger.LogInformation(
                "Transfer completed: {Amount} from AccountId {From} to AccountId {To}",
                dto.Amount, dto.FromAccountId, dto.ToAccountId);

            var created = await _transactionRepository.GetByIdAsync(transactionId);
            return MapToDto(created!);
        }
        catch
        {
            await sqlTransaction!.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<TransactionResponseDto>> GetByAccountIdAsync(int accountId, int customerId)
    {
        // Authorization check — πριν επιστρέψουμε transactions
        var account = await _accountRepository.GetByIdAsync(accountId);

        if (account == null)
            throw new KeyNotFoundException("Account not found.");

        if (account.CustomerId != customerId)
            throw new UnauthorizedAccessException("Access denied.");

        var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);
        return transactions.Select(MapToDto);
    }

    private static TransactionResponseDto MapToDto(Transaction t) => new()
    {
        TransactionId = t.TransactionId,
        FromAccountId = t.FromAccountId,
        ToAccountId = t.ToAccountId,
        Amount = t.Amount,
        TransactionType = t.TransactionType,
        Status = t.Status,
        Description = t.Description,
        CreatedAt = t.CreatedAt
    };
}
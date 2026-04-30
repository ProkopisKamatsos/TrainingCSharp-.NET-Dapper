namespace BankingApi.DTOs.Account;

public class CreateAccountDto
{
    public string AccountType { get; set; } = string.Empty; // Checking | Savings
}
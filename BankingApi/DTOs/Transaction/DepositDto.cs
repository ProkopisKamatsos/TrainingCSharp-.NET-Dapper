namespace BankingApi.DTOs.Transaction;

public class DepositDto
{
    public int ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
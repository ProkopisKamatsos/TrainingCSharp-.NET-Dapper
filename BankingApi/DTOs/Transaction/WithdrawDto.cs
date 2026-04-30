namespace BankingApi.DTOs.Transaction;

public class WithdrawDto
{
    public int FromAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
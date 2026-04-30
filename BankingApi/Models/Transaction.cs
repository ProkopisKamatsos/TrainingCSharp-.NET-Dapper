namespace BankingApi.Models;

public class Transaction
{
    public int TransactionId { get; set; }
    public int? FromAccountId { get; set; }  // nullable — NULL σε deposit
    public int? ToAccountId { get; set; }    // nullable — NULL σε withdrawal
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty; // Deposit | Withdrawal | Transfer
    public string Status { get; set; } = string.Empty;          // Completed | Failed
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
namespace BankingApi.Domain;

public class BankAccount
{
    public int Id { get; private set; }
    public decimal Balance { get; private set; }

    // Για EF Core (να μπορεί να δημιουργεί object από DB)
    private BankAccount() { }

    // Για δικό μας code (business)
    public BankAccount(int id)
    {
        Id = id;
        Balance = 0m;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.");

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
    }
}

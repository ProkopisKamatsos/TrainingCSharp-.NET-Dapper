namespace Data_M1;

public interface IBankAccount
{
    int AccountNumber { get; }
    string CustomerId { get; }
    double Balance { get; }
    string AccountType { get; }

    void Deposit(double amount);
    bool Withdraw(double amount);
    bool Transfer(IBankAccount targetAccount, double amount);
    void ApplyInterest(double years);
    void ApplyRefund(double refund);
    bool IssueCashiersCheck(double amount);
    string DisplayAccountInfo();
}

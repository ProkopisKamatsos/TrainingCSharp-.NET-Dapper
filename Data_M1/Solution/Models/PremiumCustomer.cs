using System;

namespace Data_M1;

public class PremiumCustomer : BankCustomer
{
    private const decimal MinimumCombinedBalance = 100000;

    public PremiumCustomer(string firstName, string lastName) : base(firstName, lastName)
    {
    }
    
    public override bool IsPremiumCustomer()
    {
        if (GetCombinedBalance() >= MinimumCombinedBalance && HasRequiredAccountTypes())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ApplyBenefits()
    {
        if (this.IsPremiumCustomer())
        {
            // logic to apply benefits for premium customers
            Console.WriteLine("Congratulations! Your premium customer benefits include:");
            Console.WriteLine(" - Dedicated customer service line");
            Console.WriteLine(" - Free overdraft protection for your checking account");
            Console.WriteLine(" - Higher interest rates on savings accounts");
            Console.WriteLine(" - Free wire transfers and cashier's checks");
            Console.WriteLine(" - Free safe deposit box rental");

            // logic to apply account-specific benefits

        }
        else
        {
            Console.WriteLine("See a manager to learn about our premium accounts.");
        }
    }

    private bool HasRequiredAccountTypes()
    {
        // logic to check for required account types, must have: Checking, Savings, and MoneyMarket
        return true;
    }

    private decimal GetCombinedBalance()
    {
        // logic to return the combined balance of all accounts belonging to the customer
        decimal combinedBalance = 200000;
        return combinedBalance;
    }
}

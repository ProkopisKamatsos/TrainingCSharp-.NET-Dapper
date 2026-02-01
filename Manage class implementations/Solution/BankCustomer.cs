sing System;

namespace Classes_M3;

public partial class BankCustomer
{
    private static int s_nextCustomerId;
    private string _firstName = "Tim";
    private string _lastName = "Shao";
    public readonly string CustomerId;

    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; }
    }

    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; }
    }

    static BankCustomer()
    {
        Random random = new Random();
        s_nextCustomerId = random.Next(10000000, 20000000);
    }

    public BankCustomer(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        this.CustomerId = (s_nextCustomerId++).ToString("D10");
    }
        // Copy constructor for BankCustomer
    public BankCustomer(BankCustomer existingCustomer)
    {

        this.FirstName = existingCustomer.FirstName;
        this.LastName = existingCustomer.LastName;
        //this.CustomerId = existingCustomer.CustomerId;
        this.CustomerId = (s_nextCustomerId++).ToString("D10");

    }
}

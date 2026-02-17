using DapperMvcDemo.Models;
using Z.Dapper.Plus;

public static class DapperPlusMapping
{
    private static bool _mapped = false;

    public static void EnsureMapped()
    {
        if (_mapped) return;

        DapperPlusManager.Entity<Customer>()
            .Table("Customers")
            .Identity(x => x.CustomerID);

        _mapped = true;
    }
}

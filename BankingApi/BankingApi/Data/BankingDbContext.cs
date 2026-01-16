using BankingApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options)
        : base(options)
    {
    }

    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
}

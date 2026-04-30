using BankingApi.Models;

namespace BankingApi.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int customerId);
    Task<Customer?> GetByEmailAsync(string email);
    Task<int> CreateAsync(Customer customer);
}
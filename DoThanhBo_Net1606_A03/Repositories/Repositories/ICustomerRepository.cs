using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> LoginAsync(String email, String password);
        Task<List<Customer>?> GetAllCustomersAsync();
        Task<List<Customer>?> SearchCustomersAsync(string keyword);
        Task<Customer?> GetCustomerAsync(int id);
        Task UpdateCustomerAsync(Customer customer);
    }
}

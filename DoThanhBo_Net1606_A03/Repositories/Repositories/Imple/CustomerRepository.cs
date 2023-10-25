using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessObjects.DAO;

namespace Repositories.Repositories.Imple
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDAO customerDAO;

        public CustomerRepository(CustomerDAO customerDAO)
        {
            this.customerDAO = customerDAO;
        }
        public async Task<Customer?> LoginAsync(String email, String password)
            => await customerDAO.LoginAsync(email, password);
        public async Task<List<Customer>?> GetAllCustomersAsync()
            => await customerDAO.GetAllCustomersAsync();

        public async Task<List<Customer>?> SearchCustomersAsync(string keyword)
            => await customerDAO.SearchCustomersAsync(keyword);
        public async Task<Customer?> GetCustomerAsync(int id)
            => await customerDAO.GetCustomerAsync(id);
        public async Task UpdateCustomerAsync(Customer customer)
            => await customerDAO.UpdateCustomerAsync(customer);
        public async Task DeleteAsync(int customerId)
            => await customerDAO.DeleteAsync(customerId);
        public async Task<bool> CheckForDuplicateEmail(String email)
            => await customerDAO.CheckForDuplicateEmail(email);
        public async Task AddCustomerAsync(Customer customer)
            => await customerDAO.AddCustomerAsync(customer);
    }
}

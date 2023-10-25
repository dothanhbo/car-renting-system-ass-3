using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAcessObjects.DAO
{
    public class CustomerDAO
    {
        public async Task<Customer?> LoginAsync(string email, string password)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var loginCustomer = await context.Customers.FirstOrDefaultAsync(od => od.Email.ToLower().Equals(email.ToLower()) && od.Password.Equals(password) && od.CustomerStatus == 1);
                return loginCustomer;
            }
        }
        public async Task<List<Customer>?> GetAllCustomersAsync()
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var customers = await context.Customers.ToListAsync();
                return customers;
            }
        }
        public async Task<List<Customer>?> SearchCustomersAsync(string keyword)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var customers = await context.Customers.Where(x => x.CustomerName.ToLower().Contains(keyword.ToLower())).ToListAsync();
                return customers;
            }
        }
        public async Task<Customer?> GetCustomerAsync(int id)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var customers = await context.Customers.Where(x => x.CustomerId == id).ToListAsync();
                return customers[0];
            }
        }
        public async Task UpdateCustomerAsync(Customer customer)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var _customer = context.Customers.FirstOrDefault(od => od.CustomerId == customer.CustomerId);
                if (_customer != null)
                {
                    context.Entry(_customer).CurrentValues.SetValues(customer);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task DeleteAsync(int customerId)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var customer = await context.Customers.Where(x => x.CustomerId == customerId).ToListAsync();
                context.Entry(customer[0]).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> CheckForDuplicateEmail(String email)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var customer = await context.Customers.FirstOrDefaultAsync(od => od.Email == email);
                if (customer == null)
                    return true;
                else
                    return false;
            }
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
            }
        }
    }
}

using BusinessObjects.Models;
using DataAcessObjects.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Imple
{
    public class RentingTransactionRepository : IRentingTransactionRepository
    {
        private readonly RentingTransactionDAO rentingTransactionDAO;
        private readonly CarInformationDAO carInformationDAO;
        private readonly CustomerDAO customerDAO;

        public RentingTransactionRepository(RentingTransactionDAO rentingTransactionDAO, CarInformationDAO carInformationDAO, CustomerDAO customerDAO)
        {
            this.rentingTransactionDAO = rentingTransactionDAO;
            this.carInformationDAO = carInformationDAO;
            this.customerDAO = customerDAO;
        }
        public async Task<List<RentingTransaction>?> GetAllRentingTransactionsAsync()
        { 
            var rentingTransactions = await rentingTransactionDAO.GetAllRentingTransactionsAsync();
            if (rentingTransactions != null)
            {
                foreach (RentingTransaction rentingTransaction in rentingTransactions) 
                {
                    rentingTransaction.Customer = await customerDAO.GetCustomerAsync(rentingTransaction.CustomerId);
                }
            }
            return rentingTransactions;
        }
        public async Task<List<RentingTransaction>?> GetRentingTransactionsByCustomerId(int customerId)
        {
            var rentingTransactions = await rentingTransactionDAO.GetRentingTransactionsByCustomerId(customerId);
            if (rentingTransactions != null)
            {
                foreach (RentingTransaction rentingTransaction in rentingTransactions)
                {
                    rentingTransaction.Customer = await customerDAO.GetCustomerAsync(rentingTransaction.CustomerId);
                }
            }
            return rentingTransactions;
        }
        public async Task<List<RentingTransaction>?> SearchRentingTransactions(DateTime startDate, DateTime endDate)
        {
            var rentingTransactions = await rentingTransactionDAO.SearchRentingTransactions(startDate, endDate);
            if (rentingTransactions != null)
            {
                foreach (RentingTransaction rentingTransaction in rentingTransactions)
                {
                    rentingTransaction.Customer = await customerDAO.GetCustomerAsync(rentingTransaction.CustomerId);
                }
            }
            return rentingTransactions;
        }
        public async Task<int> GetNextRentingTransactionIdAsync()
            => await rentingTransactionDAO.GetNextRentingTransactionIdAsync();
        public async Task AddRentingTransactionAsync(RentingTransaction rentingTransaction)
            => await rentingTransactionDAO.AddRentingTransactionAsync(rentingTransaction);
    }
}

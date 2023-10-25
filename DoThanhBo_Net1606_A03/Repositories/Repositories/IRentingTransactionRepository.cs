using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public interface IRentingTransactionRepository
    {
        Task<List<RentingTransaction>?> GetAllRentingTransactionsAsync();
        Task<List<RentingTransaction>?> GetRentingTransactionsByCustomerId(int customerId);
        Task<List<RentingTransaction>?> SearchRentingTransactions(DateTime startDate, DateTime endDate);
        Task<int> GetNextRentingTransactionIdAsync();
        Task AddRentingTransactionAsync(RentingTransaction rentingTransaction);
    }
}

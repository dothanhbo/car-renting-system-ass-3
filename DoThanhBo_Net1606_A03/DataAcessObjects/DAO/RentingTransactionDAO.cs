using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessObjects.DAO
{
    public class RentingTransactionDAO
    {
        public async Task<List<RentingTransaction>?> GetAllRentingTransactionsAsync()
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var rentingTransactions = await context.RentingTransactions.ToListAsync();
                return rentingTransactions;
            }
        }
        public async Task<List<RentingTransaction>?> GetRentingTransactionsByCustomerId(int customerId)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var rentingTransactions = await context.RentingTransactions.Where(x => x.CustomerId == customerId).ToListAsync();
                return rentingTransactions;
            }
        }
    }
}

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
        public async Task<List<RentingTransaction>?> SearchRentingTransactions(DateTime startDate, DateTime endDate)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var rentingTransactions = await context.RentingTransactions.Where(od => od.RentingDate >= startDate && od.RentingDate <= endDate).OrderByDescending(od => od.RentingDate).ThenByDescending(od => od.TotalPrice).ToListAsync();
                return rentingTransactions;
            }
        }

        public async Task<int> GetNextRentingTransactionIdAsync()
        {

            int nextId = 1;
            using (var context = new FUCarRentingManagementContext())
            {
                var maxId = await context.RentingTransactions.MaxAsync(rt => (int?)rt.RentingTransationId);
                if (maxId.HasValue)
                {
                    nextId = maxId.Value + 1;
                }
            }
            return nextId;
        }
        public async Task AddRentingTransactionAsync(RentingTransaction rentingTransaction)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                await context.RentingTransactions.AddAsync(rentingTransaction);
                await context.SaveChangesAsync();
            }
        }
    }
}

using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessObjects.DAO
{
    public class RentingDetailDAO
    {
        public async Task<List<RentingDetail>?> GetRentingDetailsByTransactionId(int transactionId)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var rentingDetails = await context.RentingDetails.Where(x => x.RentingTransactionId == transactionId).ToListAsync();
                return rentingDetails;
            }
        }
    }
}

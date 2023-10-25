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
        public async Task AddRentingDetailAsync(RentingDetail rentingDetail)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                await context.RentingDetails.AddAsync(rentingDetail);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<bool>> CheckCarAvalable(List<RentingDetail> rentingDetails)
        {
            List<bool> check = new List<bool>();
            using (var context = new FUCarRentingManagementContext())
            {
                foreach (RentingDetail detail in rentingDetails)
                {
                    var rentingDetailList = await context.RentingDetails.Where(od => od.CarId == detail.CarId && od.EndDate >= DateTime.Today).ToListAsync();
                    foreach (var detailInList in rentingDetailList)
                    {
                        if ((detail.StartDate.Date < detailInList.StartDate.Date && detail.EndDate.Date < detailInList.StartDate.Date) ||
                            (detail.StartDate.Date > detailInList.EndDate.Date && detail.EndDate.Date > detailInList.EndDate.Date))
                        { }
                        else
                        {
                            check.Add(false);
                            break;
                        }
                        check.Add(true);
                        break;
                    }
                }
            }
            return check;
        }
    }
}

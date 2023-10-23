using BusinessObjects.Models;
using DataAcessObjects.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Imple
{
    public class RentingDetailRepository : IRentingDetailRepository
    {
        private readonly RentingDetailDAO rentingDetailDAO;
        private readonly CarInformationDAO carInformationDAO;
        public RentingDetailRepository(RentingDetailDAO rentingDetailDAO, CarInformationDAO carInformationDAO)
        {
            this.rentingDetailDAO = rentingDetailDAO;
            this.carInformationDAO = carInformationDAO;

        }
        public async Task<List<RentingDetail>?> GetRentingDetailsByTransactionId(int transactionId) 
        {
            var rentingDetails =await rentingDetailDAO.GetRentingDetailsByTransactionId(transactionId);
            if (rentingDetails != null)
            {
                foreach(RentingDetail rentingDetail in rentingDetails)
                {
                    rentingDetail.Car = await carInformationDAO.GetCarAsync(rentingDetail.CarId);
                }
            }
            return rentingDetails;
        }
    }
}

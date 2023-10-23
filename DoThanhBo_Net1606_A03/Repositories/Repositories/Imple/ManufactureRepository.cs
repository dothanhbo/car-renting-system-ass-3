using BusinessObjects.Models;
using DataAcessObjects.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Imple
{
    public class ManufactureRepository : IManufactureRepository
    {
        private readonly ManufacturerDAO manufactureDAO;

        public ManufactureRepository(ManufacturerDAO manufactureDAO)
        {
            this.manufactureDAO = manufactureDAO;
        }
        public async Task<List<Manufacturer>?> GetAllManufacturersAsync()
            => await manufactureDAO.GetAllManufacturersAsync();
    }
}

using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessObjects.DAO
{
    public class ManufacturerDAO
    {
        public async Task<List<Manufacturer>> GetAllManufacturersAsync()
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var manufacturers = await context.Manufacturers.ToListAsync();
                return manufacturers;
            }
        }
        public async Task<Manufacturer> GetManufacturerAsync(int id)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var manufacturers = await context.Manufacturers.Where(x => x.ManufacturerId == id).ToListAsync();
                return manufacturers[0];
            }
        }
    }
}

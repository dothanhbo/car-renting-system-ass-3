using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessObjects.DAO
{
    public class SupplierDAO
    {
        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var suppliers = await context.Suppliers.ToListAsync();
                return suppliers;
            }
        }
        public async Task<Supplier> GetSupplierAsync(int id)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var suppliers = await context.Suppliers.Where(x => x.SupplierId == id).ToListAsync();
                return suppliers[0];
            }
        }
    }
}

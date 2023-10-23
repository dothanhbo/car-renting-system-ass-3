using BusinessObjects.Models;
using DataAcessObjects.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Imple
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SupplierDAO supplierDAO;

        public SupplierRepository(SupplierDAO supplierDAO)
        {
            this.supplierDAO = supplierDAO;
        }
        public async Task<List<Supplier>?> GetAllSuppliersAsync()
            => await supplierDAO.GetAllSuppliersAsync();

    }
}

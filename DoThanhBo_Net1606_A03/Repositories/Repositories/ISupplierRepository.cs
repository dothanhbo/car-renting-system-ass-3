using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>?> GetAllSuppliersAsync();
    }
}

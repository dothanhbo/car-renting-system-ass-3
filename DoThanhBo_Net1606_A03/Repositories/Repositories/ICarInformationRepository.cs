using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public interface ICarInformationRepository
    {
        Task<List<CarInformation>?> GetAllCarsAsync();
        Task<CarInformation?> GetCarAsync(int id);
        Task CreateCarAsync(CarInformation carInformation);
        Task UpdateCarAsync(CarInformation carInformation);
        Task DeleteCarAsync(int carId);
        Task<List<CarInformation>?> SearchCarsAsync(string keyword);
    }
}

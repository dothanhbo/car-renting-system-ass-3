using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessObjects.DAO
{
    public class CarInformationDAO
    {
        public async Task<List<CarInformation>?> GetAllCarsAsync()
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var cars = await context.CarInformations.ToListAsync();
                return cars;
            }
        }
        public async Task<CarInformation?> GetCarAsync(int id)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var cars = await context.CarInformations.Where(x => x.CarId == id).ToListAsync();
                return cars[0];
            }
        }
        public async Task CreateCarAsync(CarInformation carInformation)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                await context.CarInformations.AddAsync(carInformation);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateCarAsync(CarInformation carInformation)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var _carInformation = context.CarInformations.FirstOrDefault(od => od.CarId == carInformation.CarId);
                if (_carInformation != null)
                {
                    context.Entry(_carInformation).CurrentValues.SetValues(carInformation);
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task DeleteCarAsync(int carId)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var existingCarInformation = await context.CarInformations.FirstOrDefaultAsync(od => od.CarId == carId);
                context.CarInformations.Remove(existingCarInformation);
                await context.SaveChangesAsync();
            }
        }
        public async Task<List<CarInformation>?> SearchCarsAsync(string keyword)
        {
            using (var context = new FUCarRentingManagementContext())
            {
                var cars = await context.CarInformations.Where(x => x.CarName.ToLower().Contains(keyword.ToLower())).ToListAsync();
                return cars;
            }
        }
    }
}

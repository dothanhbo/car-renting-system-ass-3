using BusinessObjects.Models;
using DataAcessObjects.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Imple
{
    public class CarInformationRepository : ICarInformationRepository
    {
        private readonly CarInformationDAO carInformationDAO;
        private readonly SupplierDAO supplierDAO;
        private readonly ManufacturerDAO manufacturerDAO;

        public CarInformationRepository(CarInformationDAO carInformationDAO, SupplierDAO supplierDAO, ManufacturerDAO manufacturerDAO)
        {
            this.carInformationDAO = carInformationDAO;
            this.supplierDAO = supplierDAO;
            this.manufacturerDAO = manufacturerDAO;
        }
        public async Task<List<CarInformation>?> GetAllCarsAsync()
        {
            var cars = await carInformationDAO.GetAllCarsAsync();
            if (cars != null)
            {
                foreach (CarInformation car in cars)
                {
                    car.Manufacturer = await manufacturerDAO.GetManufacturerAsync(car.ManufacturerId);
                    car.Supplier = await supplierDAO.GetSupplierAsync(car.SupplierId);
                }
                
            }
            return cars;
        }

        public async Task<CarInformation?> GetCarAsync(int id)
        {
            var car = await carInformationDAO.GetCarAsync(id);
            if (car != null)
            {
                car.Manufacturer = await manufacturerDAO.GetManufacturerAsync(car.ManufacturerId);
                car.Supplier = await supplierDAO.GetSupplierAsync(car.SupplierId);
            }
            return car;
        }
        

        public async Task CreateCarAsync(CarInformation carInformation)
            => await carInformationDAO.CreateCarAsync(carInformation);

        public async Task UpdateCarAsync(CarInformation carInformation)
            => await carInformationDAO.UpdateCarAsync(carInformation);

        public async Task DeleteCarAsync(int carId)
            => await carInformationDAO.DeleteCarAsync(carId);

        public async Task<List<CarInformation>?> SearchCarsAsync(string keyword)
        {
            var cars = await carInformationDAO.SearchCarsAsync(keyword);
            if (cars != null)
            {
                foreach (CarInformation car in cars)
                {
                    car.Manufacturer = await manufacturerDAO.GetManufacturerAsync(car.ManufacturerId);
                    car.Supplier = await supplierDAO.GetSupplierAsync(car.SupplierId);
                }

            }
            return cars;
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;
using FUCarRentingSystem.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;

namespace FUCarRentingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarInformationController : ControllerBase
    {
        private readonly ICarInformationRepository carInformationRepository;
        private readonly IMapper mapper;

        public CarInformationController(ICarInformationRepository carInformationRepository, IMapper mapper)
        {
            this.carInformationRepository = carInformationRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCarInformationsAsync()
        {
            var result = await carInformationRepository.GetAllCarsAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCarInformationAsync(int id)
        {
            var result = await carInformationRepository.GetCarAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCarAsync(CarInformationDto carInformationDto)
        {
            var car = mapper.Map<CarInformation>(carInformationDto);
            await carInformationRepository.CreateCarAsync(car);
            return Ok();
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCarAsync(CarInformationDto carInformationDto)
        {
            var car = mapper.Map<CarInformation>(carInformationDto);
            await carInformationRepository.UpdateCarAsync(car);
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCarAsync(int id)
        {
            await carInformationRepository.DeleteCarAsync(id);
            return Ok();
        }
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchCarsAsync(string keyword)
        {
            var result = await carInformationRepository.SearchCarsAsync(keyword);
            return Ok(result);
        }
    }
}

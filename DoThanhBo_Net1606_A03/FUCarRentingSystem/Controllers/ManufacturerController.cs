using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;

namespace FUCarRentingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufactureRepository manufactureRepository;
        private readonly IMapper mapper;

        public ManufacturerController(IManufactureRepository manufactureRepository, IMapper mapper)
        {
            this.manufactureRepository = manufactureRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllManufacturersAsync()
        {
            var result = await manufactureRepository.GetAllManufacturersAsync();
            return Ok(result);
        }
    }
}

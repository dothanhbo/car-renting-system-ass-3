using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;
using Repositories.Repositories.Imple;
using FUCarRentingSystem.DTO;
namespace FUCarRentingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentingDetailController : ControllerBase
    {
        private readonly IRentingDetailRepository rentingDetailRepository;
        private readonly IMapper mapper;

        public RentingDetailController(IRentingDetailRepository rentingDetailRepository, IMapper mapper)
        {
            this.rentingDetailRepository = rentingDetailRepository;
            this.mapper = mapper;
        }
        [HttpGet("{transactionId}")]
        [Authorize]
        public async Task<IActionResult> GetRentingDetailsByTransactionId(int transactionId)
        {
            var result = await rentingDetailRepository.GetRentingDetailsByTransactionId(transactionId);
            return Ok(result);
        }
        [HttpPost("check")]
        [Authorize]
        public async Task<IActionResult> CheckCarAvalable(List<RentingDetailDto> rentingDetailDto)
        {
            var rentingDetail = mapper.Map<List<RentingDetail>>(rentingDetailDto);
            var result = await rentingDetailRepository.CheckCarAvalable(rentingDetail);
            return Ok(result);
        }
    }
}

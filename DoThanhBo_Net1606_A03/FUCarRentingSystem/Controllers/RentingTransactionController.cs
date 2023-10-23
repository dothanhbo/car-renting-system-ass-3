using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;

namespace FUCarRentingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentingTransactionController : ControllerBase
    {
        private readonly IRentingTransactionRepository rentingTransactionRepository;
        private readonly IMapper mapper;

        public RentingTransactionController(IRentingTransactionRepository rentingTransactionRepository, IMapper mapper)
        {
            this.rentingTransactionRepository = rentingTransactionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRentingTransactionsAsync()
        {
            var result = await rentingTransactionRepository.GetAllRentingTransactionsAsync();
            return Ok(result);
        }
        [HttpGet("{customerId}")]
        [Authorize]
        public async Task<IActionResult> GetRentingTransactionsByCustomerId(int customerId)
        {
            var result = await rentingTransactionRepository.GetRentingTransactionsByCustomerId(customerId);
            return Ok(result);
        }
    }
}

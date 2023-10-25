using AutoMapper;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories;
using FUCarRentingSystem.DTO;
namespace FUCarRentingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentingTransactionController : ControllerBase
    {
        private readonly IRentingTransactionRepository rentingTransactionRepository;
        private readonly IRentingDetailRepository rentingDetailRepository;
        private readonly IMapper mapper;

        public RentingTransactionController(IRentingTransactionRepository rentingTransactionRepository, IRentingDetailRepository rentingDetailRepository, IMapper mapper)
        {
            this.rentingTransactionRepository = rentingTransactionRepository;
            this.rentingDetailRepository = rentingDetailRepository;
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
        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchRentingTransactions(DateTime startDate, DateTime endDate)
        {
            var result = await rentingTransactionRepository.SearchRentingTransactions(startDate, endDate);
            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRentingTransaction(RentingTransactionDto rentingTransactionDto)
        {
            int nextId = await rentingTransactionRepository.GetNextRentingTransactionIdAsync();
            var rentingTransaction = mapper.Map<RentingTransaction>(rentingTransactionDto);
            rentingTransaction.RentingTransationId = nextId;
            await rentingTransactionRepository.AddRentingTransactionAsync(rentingTransaction);
            foreach (var rentingDetailDto in rentingTransactionDto.RentingDetails)
            {
                var rentingDetail = mapper.Map<RentingDetail>(rentingTransactionDto);
                rentingDetail.RentingTransactionId = nextId;
                await rentingDetailRepository.AddRentingDetailAsync(rentingDetail);
            }
            return Ok();
        }
    }
}

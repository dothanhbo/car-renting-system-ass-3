using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repositories.Imple;
using FUCarRentingSystem.DTO;
using Repositories.Repositories;
using AutoMapper;
using FUCarRentingSystem.Utils;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.Models;
using DataAcessObjects.DAO;

namespace FUCarRentingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            string adminEmail = config["AdminAccount:Email"];
            string adminPassword = config["AdminAccount:Password"];
            if (loginDto.email.ToLower().Equals(adminEmail.ToLower()) && loginDto.password.Equals(adminPassword))
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = GenerateJWTString.GenerateJsonWebTokenForAdmin(adminEmail, config["AppSettings:SecretKey"], DateTime.Now)
                }); ;
            var customer = await customerRepository.LoginAsync(loginDto.email, loginDto.password);
            if (customer == null)
            {
                return Unauthorized();
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateJWTString.GenerateJsonWebToken(customer, config["AppSettings:SecretKey"], DateTime.Now)
            }); ;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var result = await customerRepository.GetAllCustomersAsync();
            return Ok(result);
        }
        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchCustomersAsync(string keyword)
        {
            var result = await customerRepository.SearchCustomersAsync(keyword);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await customerRepository.GetCustomerAsync(id);
            return Ok(result);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerAsync(CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            await customerRepository.UpdateCustomerAsync(customer);
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await customerRepository.DeleteAsync(id);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            string adminEmail = config["AdminAccount:Email"];
            if (customerRepository.CheckForDuplicateEmail(customer.Email).Result == false || customer.Email.ToLower().Equals(adminEmail.ToLower()))
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "This Email has been used!"
                });
            await customerRepository.AddCustomerAsync(customer);
            return Ok();
        }
    }
}

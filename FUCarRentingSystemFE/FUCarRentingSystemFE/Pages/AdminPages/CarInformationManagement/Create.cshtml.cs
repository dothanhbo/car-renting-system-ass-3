using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;

namespace FUCarRentingSystemFE.Pages.AdminPages.CarInformationManagement
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty]
        public byte CarStatus { get; set; } = 1;
        public async Task<IActionResult> OnGetAsync()
        {
            string adminId = HttpContext.Session.GetString("Admin") ?? null;
            if (adminId != "Admin")
                return RedirectToPage("/NotAuthorized");
            string token = HttpContext.Session.GetString("token") ?? null;
            List<Manufacturer> manufacturers = new List<Manufacturer>();        
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/");
                var response = await client.GetAsync("Manufacturer");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    manufacturers = JsonConvert.DeserializeObject<List<Manufacturer>>(responseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve manufacturers.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }


            List<Supplier> suppliers = new List<Supplier>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/");
                var response = await client.GetAsync("Supplier");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    suppliers = JsonConvert.DeserializeObject<List<Supplier>>(responseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve suppliers.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }
            ViewData["ManufacturerId"] = new SelectList(manufacturers, "ManufacturerId", "ManufacturerName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "SupplierName");
            return Page();
        }

        [BindProperty]
        public CarInformation CarInformation { get; set; } = default!;
       
        public async Task<IActionResult> OnPostAsync()
        {
            string token = HttpContext.Session.GetString("token") ?? null;
            CarInformation.CarStatus = CarStatus;
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/");
                var carInfoJson = JsonConvert.SerializeObject(CarInformation);
                var content = new StringContent(carInfoJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("CarInformation", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve suppliers.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }
            TempData["SuccessMessage"] = "Car "+ CarInformation.CarName +" created successfully.";
            return RedirectToPage("./Index");
        }
    }
}

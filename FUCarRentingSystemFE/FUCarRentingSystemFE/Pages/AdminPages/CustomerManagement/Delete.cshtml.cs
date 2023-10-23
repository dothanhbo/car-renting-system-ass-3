using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Newtonsoft.Json;

namespace FUCarRentingSystemFE.Pages.AdminPages.CustomerManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
      public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string adminId = HttpContext.Session.GetString("Admin") ?? null;
            if (adminId != "Admin")
                return RedirectToPage("/NotAuthorized");
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:5173/api/"); // Adjust the base URL as needed.

                // Send an HTTP GET request to retrieve the list of CarInformation from the API.
                var response = await client.GetAsync("Customer/id?id=" + id.ToString()); // Use the appropriate endpoint URL.

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a JSON string.
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a list of CarInformation.
                    Customer = JsonConvert.DeserializeObject<Customer>(responseContent);
                }
                else
                {
                    // Handle the error accordingly.
                    ModelState.AddModelError(string.Empty, "Failed to retrieve car information.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call.
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5173/api/"); // Adjust the base URL as needed.

            // Send an HTTP GET request to retrieve the list of CarInformation from the API.
            var response = await client.DeleteAsync("Customer?carId=" + Customer.CustomerId.ToString()); // Use the appropriate endpoint URL

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Customer deleted successfully.";
            }
            return RedirectToPage("/AdminPages/CustomerManagement/Index");
        }
    }
}

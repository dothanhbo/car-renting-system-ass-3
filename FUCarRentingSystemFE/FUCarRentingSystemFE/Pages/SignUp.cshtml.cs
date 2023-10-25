using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using BusinessObjects.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FUCarRentingSystemFE.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SignUpModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:5071/api/"); // Adjust the base URL as needed.
                // Send an HTTP GET request to retrieve the list of CarInformation from the API.
                Customer.CustomerStatus = 1;
                var customerInfoJson = JsonConvert.SerializeObject(Customer);
                var content = new StringContent(customerInfoJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Customer", content);
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a JSON string.
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a list of CarInformation.
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Assuming the response contains specific error details, you can deserialize it if needed.
                    var errorDetails = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

                    // Add model errors based on the error details received.
                    ModelState.AddModelError(string.Empty, errorDetails.Message);
                    return Page();
                }
                else
                {
                    // Handle other error cases here.
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call.
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            };
            return RedirectToPage("./Index");
        }
    }
}

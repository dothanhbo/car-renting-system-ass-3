using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FUCarRentingSystemFE.Pages.AdminPages.CarInformationManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
      public CarInformation CarInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string adminId = HttpContext.Session.GetString("Admin") ?? null;
            if (adminId != "Admin")
                return RedirectToPage("/NotAuthorized");
            string token = HttpContext.Session.GetString("token") ?? null;
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/");
                var response = await client.GetAsync("CarInformation/" + id.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    CarInformation = JsonConvert.DeserializeObject<CarInformation>(responseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve car information.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string token = HttpContext.Session.GetString("token") ?? null;
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri("http://localhost:5071/api/");
            var response = await client.DeleteAsync("CarInformation/" + CarInformation.CarId.ToString());

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Car information deleted successfully.";
            }
            return RedirectToPage("/AdminPages/CarInformationManagement/Index");
        }
    }
}

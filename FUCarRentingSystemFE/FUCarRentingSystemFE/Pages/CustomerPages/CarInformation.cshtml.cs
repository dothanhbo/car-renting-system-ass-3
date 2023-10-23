using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using static FUCarRentingSystemFE.Pages.CustomerPages.CarInformationModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Headers;

namespace FUCarRentingSystemFE.Pages.CustomerPages
{
    public class CarInformationModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Keyword { get; set; }
        public String CustomerMessage { get; set; }
        public List<CarInformation> CarInformation { get; set; } = new List<CarInformation>();
        private readonly IHttpClientFactory _httpClientFactory;

        public CarInformationModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId") ??0;
            if (customerId == 0)
                return RedirectToPage("/NotAuthorized");
            string token = HttpContext.Session.GetString("token") ?? null;
            CustomerMessage = HttpContext.Session.GetString("CustomerMessage");
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/");
                var response = await client.GetAsync("CarInformation");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    CarInformation = JsonConvert.DeserializeObject<List<CarInformation>>(responseContent);
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
            if (Keyword == null)
                return RedirectToPage("./Index");
            else
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.BaseAddress = new Uri("http://localhost:5071/api/");
                    var response = await client.GetAsync("CarInformation/search?keyword=" + Keyword);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        CarInformation = JsonConvert.DeserializeObject<List<CarInformation>>(responseContent);
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
    } 
}

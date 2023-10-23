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

namespace FUCarRentingSystemFE.Pages.CustomerPages.RentingPages
{
    public class RentingTransactionListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RentingTransactionListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IList<RentingTransaction> RentingTransaction { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            if (customerId == 0)
                return RedirectToPage("/NotAuthorized");
            string token = HttpContext.Session.GetString("token");
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/"); 
                var response = await client.GetAsync("RentingTransaction/" + customerId.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    RentingTransaction = JsonConvert.DeserializeObject<List<RentingTransaction>>(responseContent);
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

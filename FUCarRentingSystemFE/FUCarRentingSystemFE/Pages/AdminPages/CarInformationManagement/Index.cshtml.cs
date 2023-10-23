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
using System.Net.Http.Headers;

namespace FUCarRentingSystemFE.Pages.AdminPages.CarInformationManagement
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IList<CarInformation> CarInformation { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string Keyword { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string adminId = HttpContext.Session.GetString("Admin")??null;
            if (adminId != "Admin")
                return RedirectToPage("/NotAuthorized");
            string token = HttpContext.Session.GetString("token") ?? null;
            if (TempData.ContainsKey("SuccessMessage"))
            {
                var successMessage = TempData["SuccessMessage"].ToString();
                ViewData["SuccessMessage"] = successMessage;
            }
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
            string adminId = HttpContext.Session.GetString("Admin") ?? null;
            if (adminId != "Admin")
                return RedirectToPage("/NotAuthorized");
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

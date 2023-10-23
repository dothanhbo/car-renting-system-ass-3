using BusinessObjects.Models;
using FUCarRentingSystemFE.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FUCarRentingSystemFE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginModel loginModel { get; set; }
        public IActionResult OnGet()
        {
            // Clear all user sessions
            HttpContext.Session.Clear();

            // Sign the user out

            // Redirect to the desired page after logout (e.g., home page)
            return Page(); // Change this to your desired page
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:5071/api/"); // Adjust the base URL as needed.

                var loginJson = JsonConvert.SerializeObject(loginModel);
                var content = new StringContent(loginJson, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Customer/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JObject.Parse(responseContent);
                    string token = responseObject["data"].ToString();
                    HttpContext.Session.SetString("token", token);
                    var handler = new JwtSecurityTokenHandler();
                    var jwtClaims = handler.ReadToken(token) as JwtSecurityToken;
                    if (jwtClaims.Claims.Any(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && claim.Value == "Customer"))
                    {
                        Claim serialNumberClaim = jwtClaims.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber");
                        HttpContext.Session.SetInt32("CustomerId", Convert.ToInt32(serialNumberClaim.Value));
                        return RedirectToPage("/CustomerPages/CarInformation");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Admin", "Admin");
                        return RedirectToPage("/AdminPages/CarInformationManagement/Index");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return Page();
            }
        }
    }
}
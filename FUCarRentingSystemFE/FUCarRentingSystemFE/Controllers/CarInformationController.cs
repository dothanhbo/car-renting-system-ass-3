using System.Threading.Tasks;
using BusinessObjects;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BusinessObjects.Models;
using System.Text;

namespace FUCarRentingSystemFE.Controllers
{
    public class CarInformationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarInformationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", loginModel);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:5173/api/"); // Adjust the base URL as needed.

                var loginJson = JsonConvert.SerializeObject(loginModel);
                var content = new StringContent(loginJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("Customer/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Login was successful. You can perform further actions here.
                    return RedirectToAction("Success"); // Redirect to a success action or view.
                }
                else
                {
                    // Login failed. Handle the error accordingly.
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View("Index", loginModel);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call.
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View("Index", loginModel);
            }
        }

    }
}

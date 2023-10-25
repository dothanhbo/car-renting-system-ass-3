using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using SimpleShoppingCartSession.Models;
using System.Linq;
using System.Data;
using BusinessObjects.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Net.Http.Headers;

namespace FUCarRentingSystemFE.Pages.CustomerPages
{

    public class CartModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public List<Item> cart { get; set; }
        public decimal Total { get; set; }
        public async Task<IActionResult> OnGet()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            if (customerId == 0)
                return RedirectToPage("/NotAuthorized");
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            Total = cart.Sum(i => i.CarInformation.CarRentingPricePerDay * i.TotalDays);
            return Page();

        }

            public async Task<IActionResult> OnGetBuyNow(int id)
            {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Item>();
                Item item = new Item();
                item.CarInformation = await GetCarInformationAsync(id);
                item.StartDate = DateTime.Now;
                item.EndDate = DateTime.Now;
                item.TotalDays = 1;
                item.Total = item.CarInformation.CarRentingPricePerDay * item.TotalDays;
                cart.Add(item);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = Exists(cart, id);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        CarInformation = await GetCarInformationAsync(id),
                        StartDate = DateTime.Now,
                        TotalDays =1,
                        EndDate = DateTime.Now,
                    });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToPage("Cart");
        }
        public IActionResult OnGetDelete(int id)
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = Exists(cart, id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToPage("Cart");
        }

        public void CalculateTotal()
        {
            Total = 0; // Initialize total

            foreach (var item in cart)
            {
                // Calculate the number of days between StartDate and EndDat

                // Add the subtotal to the total
                Total += item.Total;
            }
        }

        // ... other methods ...
        public IActionResult OnPostUpdate(DateTime[] startDate, DateTime[] endDate, decimal[] total)
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (var i = 0; i < cart.Count; i++)
            {
                cart[i].StartDate = startDate[i];
                cart[i].EndDate = endDate[i];
                if (cart[i].StartDate > cart[i].EndDate)
                    cart[i].TotalDays = 0;
                else
                    cart[i].TotalDays = (int)(cart[i].EndDate - cart[i].StartDate).TotalDays + 1;
                cart[i].Total = total[i];
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            CalculateTotal();
            return RedirectToPage("Cart");
        }

        public async Task<IActionResult> OnPostBuynow()
        {
            int? customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
            if (customerId == 0)
                return RedirectToPage("/NotAuthorized");
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            Total = cart.Sum(i => i.CarInformation.CarRentingPricePerDay * i.TotalDays);
            if (Total <= 0)
            {
                HttpContext.Session.SetString("CustomerMessage", "Can not rent. Nothing for rent!");
                return RedirectToPage("/CustomerPages/CarInformation");
            }
            List<RentingDetail> rentingDetails = new List<RentingDetail>();
            foreach(Item item in cart)
            {
                rentingDetails.Add(new RentingDetail
                {
                    CarId = item.CarInformation.CarId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    RentingTransactionId =1
                });
            }
            List<bool> checkList = await CheckCarExist(rentingDetails);
            if (checkList.Contains(false))
            {
                string context= "Can not rent, car ";
                for (int i = 0; i < checkList.Count; i++)
                {
                    if (checkList[i] == false)
                    {
                        context += "("+cart[i].CarInformation.CarName + ") ";
                    }
                }
                context += "had been rented for that days.";
                HttpContext.Session.SetString("CustomerMessage",context);
                return RedirectToPage("/CustomerPages/CarInformation");
            }
            RentingTransaction rentingTransaction = new RentingTransaction();
            rentingTransaction.RentingDate = DateTime.Now;
            rentingTransaction.TotalPrice = Total;
            rentingTransaction.CustomerId = customerId.Value;
            rentingTransaction.RentingStatus = 1;
            foreach(var item in cart)
            {
                //check if any car do not have day for rent
                if(item.TotalDays > 0)
                rentingTransaction.RentingDetails.Add(
                    new RentingDetail
                    {
                        CarId = item.CarInformation.CarId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Price = item.CarInformation.CarRentingPricePerDay * item.TotalDays, 
                    }
                );  
            }
            string token = HttpContext.Session.GetString("token");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri("http://localhost:5071/api/");
            var carInfoJson = JsonConvert.SerializeObject(rentingTransaction);
            var content = new StringContent(carInfoJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("RentingTransaction", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle the error accordingly.
                ModelState.AddModelError(string.Empty, "Failed to retrieve suppliers.");
            }
            HttpContext.Session.Remove("cart");
            HttpContext.Session.SetString("CustomerMessage", "Renting Success!");
            return RedirectToPage("/CustomerPages/CarInformation");
        }

        private int Exists(List<Item> cart, int id)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].CarInformation.CarId == id)
                {
                    return i;
                }
            }
            return -1;
        }

        private async Task<CarInformation> GetCarInformationAsync(int id)
        {
            CarInformation carInformation = new CarInformation();
            string token = HttpContext.Session.GetString("token");
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("http://localhost:5071/api/");
                var response = await client.GetAsync("CarInformation/" + id.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    carInformation = JsonConvert.DeserializeObject<CarInformation>(responseContent);
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
            return carInformation;
        }
        private async Task<List<bool>> CheckCarExist(List<RentingDetail> rentingDetails)
        {
            string token = HttpContext.Session.GetString("token");
            List<bool> carExist = new List<bool>();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri("http://localhost:5071/api/");
            var carInfoJson = JsonConvert.SerializeObject(rentingDetails);
            var content = new StringContent(carInfoJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("RentingDetail/check", content);
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a JSON string.
                var responseContent = await response.Content.ReadAsStringAsync();
                carExist = JsonConvert.DeserializeObject<List<bool>>(responseContent);
                // Deserialize the JSON response into a list of CarInformation.
            }
            else
            {
                // Handle the error accordingly.
                ModelState.AddModelError(string.Empty, "Failed to retrieve suppliers.");
            }
            return carExist;
        }
    }
}

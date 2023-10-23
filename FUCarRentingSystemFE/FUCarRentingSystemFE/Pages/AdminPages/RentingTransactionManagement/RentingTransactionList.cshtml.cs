using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Newtonsoft.Json;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing;
using ClosedXML.Excel;
using System.Net.Http.Headers;

namespace FUCarRentingSystemFE.Pages.AdminPages.RentingTransactionManagement
{
    public class RentingTransactionListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RentingTransactionListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [BindProperty(SupportsGet = true)]
        public DateTime? startDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? endDate { get; set; }
        public IList<RentingTransaction> RentingTransaction { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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
                var response = await client.GetAsync("RentingTransaction"); 

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
        public async Task<IActionResult> OnPostAsync()
        {
            if (startDate == null && endDate == null)
                return RedirectToPage("./Index");
            else
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri("http://localhost:5173/api/");
                    var response = await client.GetAsync("RentingTransaction/search?startDate=" + startDate.Value.ToString("MM-dd-yyyy") + "&endDate=" + endDate.Value.ToString("MM-dd-yyyy"));

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

        public IActionResult OnPostCreateReport()
        {
            var RentingDate = Request.Form["RentingDate"];
            var Price = Request.Form["Price"];
            var RentingStatus = Request.Form["RentingStatus"];
            var Customer = Request.Form["Customer"];
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet =
                workbook.Worksheets.Add("RentingTransactions");
                worksheet.Cell(1, 1).Value = "Num";
                worksheet.Cell(1, 2).Value = "Renting Date";
                worksheet.Cell(1, 3).Value = "Price";
                worksheet.Cell(1, 4).Value = "Renting Status";
                worksheet.Cell(1, 5).Value = "Customer Name";

                IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(1, 5).Address);
                range.Style.Fill.SetBackgroundColor(XLColor.Almond);
                int cellId = 1;
                for (int i = 0; i < RentingDate.Count; i++)
                {
                    cellId++;
                    worksheet.Cell(cellId, 1).Value = (cellId - 1);
                    worksheet.Cell(cellId, 2).Value = RentingDate[i];
                    worksheet.Cell(cellId, 3).Value = Double.Parse(Price[i]);
                    worksheet.Cell(cellId, 4).Value = int.Parse(RentingStatus[i]);
                    worksheet.Cell(cellId, 5).Value = Customer[i];
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    var strDate = DateTime.Now.ToString("ddMMyyyy");
                    string filename = string.Format($"RentingTransactions_{strDate}.xlsx");

                    return File(content, contentType, filename);
                }
            }
            return Page();
        }
    }
}

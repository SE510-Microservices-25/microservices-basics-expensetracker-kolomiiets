using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using ReportService.Models;

namespace ReportService.Controllers
{
    [Route("expense-report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ExpenseReport>> GetReport()
        {
            var expenseServiceUrl = "http://expenseservice:8080/new-expenses";

            var response = await _httpClient.GetAsync(expenseServiceUrl);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching expenses.");
            }

            var expensesJson = await response.Content.ReadAsStringAsync();
            var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesJson);

            var totalAmount = expenses.Sum(e => e.Amount);

            var report = new ExpenseReport
            {
                Category = "All",
                TotalAmount = totalAmount
            };

            return Ok(report);
        }
    }
}

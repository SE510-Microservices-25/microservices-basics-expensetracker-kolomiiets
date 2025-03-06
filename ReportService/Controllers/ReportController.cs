using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using ReportService.Models;
using System.Linq;

namespace ReportService.Controllers
{
    [Route("expense-tracker")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ReportController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<ExpenseReport>>> GetReport()
        {
            var expenseServiceUrl = "http://expenseservice/expense-tracker/new";

            var response = await _httpClient.GetAsync(expenseServiceUrl);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching expenses.");
            }

            var expensesJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine(expensesJson);

            var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesJson, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });

            if (expenses == null || !expenses.Any())
            {
                return NotFound("No expenses found.");
            }

            foreach (var expense in expenses)
            {
                if (string.IsNullOrEmpty(expense.Category))
                {
                    expense.Category = "Unknown";
                }
            }

            var groupedByCategory = expenses
                .GroupBy(e => e.Category)
                .Select(g => new ExpenseReport
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                }).ToList();

            return Ok(groupedByCategory);
        }
    }
}

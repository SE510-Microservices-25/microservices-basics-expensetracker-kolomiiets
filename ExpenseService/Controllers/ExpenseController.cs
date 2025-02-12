using Microsoft.AspNetCore.Mvc;
using ExpenseService.Models;

namespace ExpenseService.Controllers
{
    [Route("new-expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private static List<Expense> _expenses = new();

        [HttpGet]
        public ActionResult<IEnumerable<Expense>> GetExpenses()
        {
            return Ok(_expenses);
        }

        [HttpPost]
        public ActionResult AddExpense([FromBody] Expense expense)
        {
            expense.Id = _expenses.Count + 1;
            _expenses.Add(expense);
            return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
        }
    }
}

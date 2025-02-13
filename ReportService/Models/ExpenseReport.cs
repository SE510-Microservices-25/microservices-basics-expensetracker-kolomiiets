namespace ReportService.Models
{
    public class ExpenseReport
    {
        public string Category { get; set; }
        public decimal TotalAmount { get; set; }
    };

    public class Expense
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

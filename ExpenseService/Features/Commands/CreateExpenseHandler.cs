using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ExpenseService.Data;
using ExpenseService.Models;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, Guid>
{
    private readonly AppDbContext _context;

    public CreateExpenseHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Category = request.Category,
            Date = request.Date
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}

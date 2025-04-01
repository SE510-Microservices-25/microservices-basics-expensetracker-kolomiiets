using MediatR;
using System.Collections.Generic;
using ExpenseService.Models;

public record GetExpensesQuery() : IRequest<List<Expense>>;

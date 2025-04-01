using MediatR;

public record CreateExpenseCommand(decimal Amount, string Category, DateTime Date) : IRequest<Guid>;

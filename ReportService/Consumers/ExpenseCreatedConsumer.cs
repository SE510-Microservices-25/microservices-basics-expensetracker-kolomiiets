using MassTransit;
using Microsoft.Extensions.Logging;

public class ExpenseCreatedConsumer : IConsumer<ExpenseCreated>
{
    private readonly ILogger<ExpenseCreatedConsumer> _logger;

    public ExpenseCreatedConsumer(ILogger<ExpenseCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ExpenseCreated> context)
    {
        _logger.LogInformation($"Отримано витрату: {context.Message.Category} - {context.Message.Amount}");
        return Task.CompletedTask;
    }
}

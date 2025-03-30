using MassTransit;
using System.Threading.Tasks;

public class RabbitMqProducer
{
    private readonly IBus _bus;

    public RabbitMqProducer(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendExpenseCreatedEvent(Guid expenseId, decimal amount, string category, DateTime date)
    {
        var message = new ExpenseCreated(expenseId, amount, category, date);
        await _bus.Publish(message);
    }
}

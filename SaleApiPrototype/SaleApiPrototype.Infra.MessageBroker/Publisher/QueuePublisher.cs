using SaleApiPrototype.Domain.MessageBroker;
using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Infra.MessageBroker.Publisher;

internal class QueuePublisher : IQueuePublisher
{
    public Task<Result> PublishAsync<TEvent>(object message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Published event {typeof(TEvent).Name}.");
        return Task.FromResult(Result.Success());
    }
}

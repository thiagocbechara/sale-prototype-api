using SaleApiPrototype.Domain.Models;

namespace SaleApiPrototype.Domain.MessageBroker;

public interface IQueuePublisher
{
    Task<Result> PublishAsync<TEvent>
        (object message, CancellationToken cancellationToken);
}

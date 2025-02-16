using Microsoft.Extensions.DependencyInjection;
using SaleApiPrototype.Domain.MessageBroker;
using SaleApiPrototype.Infra.MessageBroker.Publisher;

namespace SaleApiPrototype.Infra.MessageBroker.Extensions;

public static class MessageBrokerDepencyInjectionExtensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddScoped<IQueuePublisher, QueuePublisher>();
        return services;
    }
}

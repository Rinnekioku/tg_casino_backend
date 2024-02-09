using Common.Utils.EventBus.Interfaces;
using Common.Utils.EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Utils.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddUtilityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add RabbitMQEventBus
        services.AddSingleton<IEventBus, RabbitMqEventBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMqEventBus(scopeFactory, configuration.GetSection("EventBus:Host").Value!,
                    sp.GetService<ILogger<RabbitMqEventBus>>()!
                );
            }
        );
    }
}
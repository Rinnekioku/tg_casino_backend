using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Common.Utils.EventBus.Events;
using Common.Utils.EventBus.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Utils.EventBus.RabbitMQ;

public class RabbitMqEventBus : IEventBus
{
    private delegate Task AsyncEventHandlerDelegate(string eventData);

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _hostName;
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly Dictionary<string, Dictionary<Type, AsyncEventHandlerDelegate>> _handlersRegistry;

    public RabbitMqEventBus(IServiceScopeFactory serviceScopeFactory, string hostName, ILogger<RabbitMqEventBus> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hostName = hostName;
        _logger = logger;
        _handlersRegistry = new Dictionary<string, Dictionary<Type, AsyncEventHandlerDelegate>>();
    }

    public async Task Publish<T>(T @event)
        where T : Event
    {
        await Task.Yield();

        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var eventName = @event.GetType().Name;
        channel.QueueDeclare(eventName, false, false, false, null);
        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("", eventName, null, body);
    }

    public void On<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        var eventType = typeof(T).Name;

        _handlersRegistry.TryAdd(eventType, new Dictionary<Type, AsyncEventHandlerDelegate>());
        if (!_handlersRegistry[eventType].TryAdd(typeof(TH), async (string eventData) =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var @event = JsonConvert.DeserializeObject<T>(eventData)!;
                var handler = scope.ServiceProvider.GetRequiredService<TH>();
                await handler.Handle(@event);
            }))
        {
            _logger.LogWarning("EventHandler with type {EventName} already registered", eventType);
        }

        Listen<T>();
    }

    private void Listen<T>() where T : Event
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var eventName = typeof(T).Name;
        channel.QueueDeclare(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += OnEventReceived;

        channel.BasicConsume(eventName, false, consumer);
    }

    private async Task OnEventReceived(object sender, BasicDeliverEventArgs e)
    {
        var eventType = e.RoutingKey;
        var eventData = Encoding.UTF8.GetString(e.Body.Span);

        if (_handlersRegistry.TryGetValue(eventType, out var eventHandlers))
        {
            try
            {
                foreach (var handler in eventHandlers.Values)
                {
                    await handler(eventData);
                }

                ((AsyncDefaultBasicConsumer)sender).Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something went wrong with Consumer_Received!");
            }
        }
    }
}
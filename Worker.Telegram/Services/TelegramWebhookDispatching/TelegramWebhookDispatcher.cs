using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Worker.Telegram.Services.Interfaces;
using Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

namespace Worker.Telegram.Services.TelegramWebhookDispatching;

public class TelegramWebhookDispatcher : ITelegramWebhookDispatcher
{
    private delegate Task AsyncUpdateHandlerDelegate(Update update, CancellationToken cancellationToken);

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<TelegramWebhookDispatcher> _logger;
    private readonly Dictionary<UpdateType, Dictionary<Type, AsyncUpdateHandlerDelegate>> _updateHandlersRegistry;

    public TelegramWebhookDispatcher(IServiceScopeFactory serviceScopeFactory,
        ILogger<TelegramWebhookDispatcher> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _updateHandlersRegistry = new Dictionary<UpdateType, Dictionary<Type, AsyncUpdateHandlerDelegate>>();
    }

    public void On<TH>(UpdateType updateType)
        where TH : IUpdateHandler
    {
        _updateHandlersRegistry.TryAdd(updateType, new Dictionary<Type, AsyncUpdateHandlerDelegate>());
        if (!_updateHandlersRegistry[updateType].TryAdd(typeof(TH),
                async (update, cancellationToken) =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<TH>();
                    await handler.Handle(update, cancellationToken);
                }))
        {
            _logger.LogWarning("UpdateHandler with type {UpdateHandlerType} already registered", typeof(TH).Name);
        }
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (_updateHandlersRegistry.TryGetValue(update.Type, out var updateHandlers))
        {
            try
            {
                foreach (var handler in updateHandlers.Values)
                {
                    await handler(update, cancellationToken);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Something went wrong with Consumer_Received!");
            }
        }
        else
        {
            await Task.Yield();
            UnknownUpdateHandlerAsync(update);
        }
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", errorMessage);

        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    private void UnknownUpdateHandlerAsync(global::Telegram.Bot.Types.Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
    }
}
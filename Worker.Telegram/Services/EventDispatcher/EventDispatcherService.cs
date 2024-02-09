using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Worker.Telegram.Services.EventDispatcher.Handlers;

namespace Worker.Telegram.Services.EventDispatcher;

public class EventDispatcherService : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<EventDispatcherService> _logger;
    private readonly Dictionary<UpdateType, IEventHandler> _eventHandlers = new();

    public EventDispatcherService(ITelegramBotClient botClient, ILogger<EventDispatcherService> logger)
    {
        _botClient = botClient;
        _logger = logger;

        RegisterHandlers();
    }

    private void RegisterHandlers()
    {
        _eventHandlers.Add(UpdateType.Message, new MessageHandler(_botClient));
        _eventHandlers.Add(UpdateType.CallbackQuery, new CallbackQueryHandler(_botClient));
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        if (_eventHandlers.TryGetValue(update.Type, out var handler))
        {
            await handler.Process(update, cancellationToken);
        }
        else
        {
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

    private void UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
    }
}
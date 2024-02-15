using System.Net.Http.Headers;
using System.Text;
using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;
using Newtonsoft.Json;
using Worker.Telegram.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

public class CallbackQueryHandler : UpdateHandlerBase
{
    private class TelegramLoginResponse
    {
        public string Token { get; set; }
    }

    private readonly IEventBus _eventBus;

    public CallbackQueryHandler(ITelegramBotClient botClient, IEventBus eventBus) : base(botClient)
    {
        _eventBus = eventBus;
    }

    public override async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var query = update.CallbackQuery!;
        if (query.GameShortName != "aff_casino")
        {
            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                cancellationToken: cancellationToken,
                text: "Invalid command"
            );
        }
        else
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var payload = $"{{ \"TelegramUsername\": \"{update.CallbackQuery?.From.Username}\" }}";
            HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

            var responseJsonString =
                await (await client.PostAsync($"http://api.casino:80/api/account/TelegramLogin?username", content))
                    .Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<TelegramLoginResponse>(responseJsonString);
            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: query.Id,
                cancellationToken: cancellationToken,
                url: $"https://affhub.ovh/?token={responseJson.Token}"
            );
        }
    }
}
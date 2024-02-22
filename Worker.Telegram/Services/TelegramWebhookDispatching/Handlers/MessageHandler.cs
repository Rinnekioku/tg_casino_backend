using System.Text;
using Common.Utils.DTOs.Referral;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Worker.Telegram.Enums;

namespace Worker.Telegram.Services.TelegramWebhookDispatching.Handlers;

public class MessageHandler : UpdateHandlerBase
{
    public MessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message!;
        string messageEventWithParamsString = message.Text!;
        if (!messageEventWithParamsString.StartsWith("/"))
        {
            return;
        }

        var commandAndParams = messageEventWithParamsString.Substring(1).Split(" ");
        var messageEventString = commandAndParams[0];
        var messageParamsString = commandAndParams.Length == 2 ? commandAndParams[1] : "";

        if (!Enum.TryParse(messageEventString, true, out BotMessageEvent messageEvent))
        {
            return;
        }

        switch (messageEvent)
        {
            case BotMessageEvent.Start:
            {
                await ReferPlayerAsync(update.Message?.From!.Username!, messageParamsString);
                await BotClient.SendGameAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    gameShortName: "aff_casino");
                break;
            }
            case BotMessageEvent.Play:
            {
                await ReferPlayerAsync(update.Message?.From!.Username!, messageParamsString);
                await BotClient.SendGameAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    gameShortName: "aff_casino");
                break;
            }
            case BotMessageEvent.Referral:
            {
                await BotClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    cancellationToken: cancellationToken,
                    text: await GenerateReferralLinkAsync(update.Message?.From!.Username!)
                );
                break;
            }
        }
    }

    private async Task ReferPlayerAsync(string telegramUsername, string referralCode)
    {
        if (referralCode == string.Empty) return;

        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        var payload = new TelegramReferPlayerRequest
            { TelegramUsername = telegramUsername, ReferralCode = referralCode };
        HttpContent content =
            new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var uri = "http://api.casino:80/api/internal/referral/TelegramReferPlayer";

        await client.PostAsync(uri, content);
    }

    private async Task<string> GenerateReferralLinkAsync(string telegramUsername)
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        var payload = new TelegramGenerateReferralLinkRequest
            { TelegramUsername = telegramUsername };
        HttpContent content =
            new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var uri = "http://api.casino:80/api/internal/referral/TelegramGenerateReferralLink";

        var response = await client.PostAsync(uri, content);
        var json = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<TelegramGenerateReferralLinkResponse>(json);

        return responseObject!.ReferralLink;
    }
}
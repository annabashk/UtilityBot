using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;
using Microsoft.VisualBasic;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"📏 Количество символов" , $"quantity"),
                        InlineKeyboardButton.WithCallbackData($"➕ Сумма чисел" , $"summa")
                    });

                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот может посчитать количество символов в сообщении или посчитатать сумму чисел.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Для подсчета количества символов в сообшении просто отправьте текстовое сообщение. Для подсчета суммы чисел введете числа через пробел.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
            }

            string userTextCode = _memoryStorage.GetSession(message.Chat.Id).TextCode;

            switch(userTextCode)
            {
                case "quantity":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                    return;
                case "summa":
                    int summa = 0;
                    string tx = message.Text;
                    string[] subs = tx.Split(' ');

                    foreach (string sub in subs)
                    {
                        int num;
                        bool success = int.TryParse(sub, out num);
                        if (success)
                        {
                            summa += num;
                        }
                        else await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Для подсчета суммы чисел введете числа через пробел", cancellationToken: ct);

                    }
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма равнa {summa}", cancellationToken: ct);
                    return;
            }
        }
    }
}

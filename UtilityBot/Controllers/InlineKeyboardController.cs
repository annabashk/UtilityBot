using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot;
using UtilityBot.Services;
using Telegram.Bot.Types.Enums;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).TextCode = callbackQuery.Data;

            string text = callbackQuery.Data switch
            {
                "quantity" => "Количество символов",
                "summa" => "Сумма чисел",
                _ => String.Empty
            };

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбрано - {text}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine} Можно изменить в главном меню.",
                cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}

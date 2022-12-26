using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
	public class InlineKeyboardController
	{
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;   
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).Func = callbackQuery.Data;

            string selectFunc = callbackQuery.Data switch
            {
                "sumChar" => " Подсчет символов",
                "sumNum" => " Сумма чисел",                
                _ => String.Empty
            };
           
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбранная функция - {selectFunc}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
	public class TextMessageController
	{
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        private readonly IMessageHandler _messageHandler;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, IMessageHandler messageHandler)
		{
			_telegramClient = telegramBotClient;
			_memoryStorage = memoryStorage;
			_messageHandler = messageHandler;
		}

		public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text ??= string.Empty)
            {
                case "/start":
               
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Подсчет символов" , $"sumChar"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sumNum")             
                    });
               
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот считает символы или числа.</b> {Environment.NewLine}",
                      cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                default:
                    string userFunc = _memoryStorage.GetSession(message.Chat.Id).Func;
                    var result = _messageHandler.Process(userFunc, message.Text); 
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
                    break;
            }           
        }
    }
}

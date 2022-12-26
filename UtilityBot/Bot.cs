using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using UtilityBot.Controllers;

namespace UtilityBot
{
	internal class Bot : BackgroundService
	{
        private ITelegramBotClient _telegramClient;
        private DefaultMessageController _defaultMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private TextMessageController _textMessageController;

        public Bot(ITelegramBotClient telegramClient, DefaultMessageController defaultMessageController, InlineKeyboardController inlineKeyboardController, TextMessageController textMessageController)
		{
			_telegramClient = telegramClient;
			_defaultMessageController = defaultMessageController;
			_inlineKeyboardController = inlineKeyboardController;
			_textMessageController = textMessageController;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, 
                cancellationToken: stoppingToken);

            Console.WriteLine("Бот запущен");
        }
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {            
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }
            
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);                 
                        return;
                    default: 
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
        }
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {            
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);

            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}

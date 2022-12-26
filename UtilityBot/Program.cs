using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using UtilityBot.Controllers;
using UtilityBot.Services;
using UtilityBot.Configuration;

namespace UtilityBot
{
	internal class Program
	{
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
          
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) 
                .Build(); 

            Console.WriteLine("Сервис запущен");
           
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();            
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("5865142865:AAE_F9MgwOnUMwt5VSPTALKDeo6Rv3uB-U0"));          
            services.AddHostedService<Bot>();
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5865142865:AAE_F9MgwOnUMwt5VSPTALKDeo6Rv3uB-U0",
            };
        }
    }
}
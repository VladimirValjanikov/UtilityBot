namespace UtilityBot.Services
{
	public interface IMessageHandler
	{
		string Process(string func, string text);
	}
}

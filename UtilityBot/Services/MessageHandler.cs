namespace UtilityBot.Services
{
	public class MessageHandler : IMessageHandler
	{
		public string Process(string func, string text)
		{
			switch (func)
			{
				case "sumNum":
					var arrayNum = text.Split(" ");
					double result = 0;
					foreach (var item in arrayNum)
					{
						if (double.TryParse(item, out var value))
							result += value;
						else
							return "Не удалось преобразовать в числовой тип.";
					}											
					return "Сумма чисел: " + result.ToString();

				case "sumChar":
					return "Символов вашем сообщении: " + text.Length.ToString();

				default: return text;
			}
		}
	}
}

namespace Birthdays.TgBot.Exceptions;

public class TgBotException(string message) : Exception(message)
{
    public static void ThrowIf(bool flag, string message)
    {
        if (flag)
        {
            throw new TgBotException(message);
        }
    }
}
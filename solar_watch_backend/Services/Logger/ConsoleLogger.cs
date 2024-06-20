namespace solar_watch_backend.Services.Logger;

public class ConsoleLogger : LoggerBase
{
    public override void LogMessage(string message, string type)
    {
        var entry = CreateLogEntry(message, type);
        Console.WriteLine(entry);
    }
}
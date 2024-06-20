namespace solar_watch_backend.Services.Logger;


public abstract class LoggerBase : ILogger
{
    public abstract void LogMessage(string message, string type);

    protected static string CreateLogEntry(string message, string type) => $"[{DateTime.Now}] {type}: {message}";

    public void LogInfo(string message)
    {
        LogMessage(message, "INFO");
    }

    public void LogError(string message)
    {
        LogMessage(message, "ERROR");
    }
}

namespace solar_watch_backend.Services.Logger;

public interface ILogger
{
    public void LogInfo(string message);
    public void LogError(string message);
}
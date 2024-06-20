namespace solar_watch_backend.Services.Logger;

public class FileLogger : LoggerBase
{
    public override void LogMessage(string message, string type)
    {
        string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        
        var logFileDirectory = WorkDir.Replace(@"bin\Debug\net8.0\", @"Resources\log-files");
        
        if (!Directory.Exists(logFileDirectory))
        {
            DirectoryInfo directory = Directory.CreateDirectory(logFileDirectory);
            //Console.WriteLine("Directory created!");
        }
        
        var entry = CreateLogEntry(message, type);
        
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(logFileDirectory, "log.txt"), true))
        {
            outputFile.WriteLine(entry);
        }
    }
}
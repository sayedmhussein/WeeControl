using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Sayed.MySystem.ClientService.Services
{
    public class LoggerService : ILogger, ILoggerService
    {
        private readonly string logPath;

        public LoggerService(string logPath)
        {
            this.logPath = logPath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public string GetLogFile(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void DeleteLogFile(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        #region Private Temporary Functions
        public void LogAppend(string arg, string filename = "logger.log")
        {
            lock (nameof(filename))
            {
                var file = Path.Combine(logPath, filename);
                if (File.Exists(file) == false)
                {
                    File.Create(file);
                }

                using StreamWriter sw = File.AppendText(file);
                sw.WriteLine(arg);
            }
        }

        public string DeleteAllLogs(string filename = "logger.log")
        {
            lock (nameof(filename))
            {
                var file = Path.Combine(logPath, filename);
                if (File.Exists(file))
                {
                    return File.ReadAllText(file);
                }
                else
                {
                    return "No Log File Found.";
                }
            }
        }

        public void ReadAllLogs(string filename = "logger.log")
        {
            lock (nameof(filename))
            {
                var file = Path.Combine(logPath, filename);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        #endregion
    }
}

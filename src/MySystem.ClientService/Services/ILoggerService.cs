namespace Sayed.MySystem.ClientService.Services
{
    public interface ILoggerService
    {
        void ReadAllLogs(string filename = "logger.log");
        string DeleteAllLogs(string filename = "logger.log");
    }
}
using System.Net.Http;
using System.Threading.Tasks;

namespace Sayed.MySystem.ClientService.Services
{
    public interface IClientServices
    {
        Setting Settings { get; }
        HttpClient HttpClient { get; set; }
        string AppDataPath { get; }

        void LogAppend(string argument, string filename = "logger.log");
        string LogReadAll(string filename = "logger.log");
        void LogDeleteAll(string filename = "logger.log");
    }
}
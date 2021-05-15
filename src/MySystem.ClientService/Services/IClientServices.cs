using System.Net.Http;
using System.Threading.Tasks;
using Sayed.MySystem.Shared.Configuration.Models;

namespace Sayed.MySystem.ClientService.Services
{
    public interface IClientServices
    {
        Setting Settings { get; }
        IApi Api { get; }
        HttpClient HttpClient { get; set; }
        string AppDataPath { get; }

        void LogAppend(string argument, string filename = "logger.log");
        string LogReadAll(string filename = "logger.log");
        void LogDeleteAll(string filename = "logger.log");
    }
}
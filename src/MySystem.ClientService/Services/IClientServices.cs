using System.Net.Http;

namespace Sayed.MySystem.ClientService.Services
{
    public interface IClientServices
    {
        Setting Settings { get; }
        HttpClient HttpClient { get; set; }
    }
}
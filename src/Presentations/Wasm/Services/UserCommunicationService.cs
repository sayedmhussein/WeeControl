using System.Net.Http;
using WeeControl.Presentations.FunctionalService.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class UserCommunicationService : IUserCommunication
{
    public string ServerBaseAddress
    {
        get => "https://localhost:5001/";
        set => _ = value;
    }

    public HttpClient HttpClient { get; }

    public UserCommunicationService(IHttpClientFactory factory)
    {
        HttpClient = factory.CreateClient();
    }
}
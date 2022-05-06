using System.Net.Http;
using WeeControl.Frontend.ServiceLibrary.Interfaces;

namespace WeeControl.Frontend.Wasm.Services;

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
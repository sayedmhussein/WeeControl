using System.Net.Http;
using Microsoft.JSInterop;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class EssentialDeviceServerDeviceService : DeviceStorageService, IDeviceServerCommunication
{
    public EssentialDeviceServerDeviceService(IJSRuntime jsRuntime, IHttpClientFactory factory) : base(jsRuntime)
    {
        HttpClient = factory.CreateClient();
    }
    
    public HttpClient HttpClient { get; }
    public string GetFullAddress(string relative)
    {
        return "https://localhost:5001/" + relative;
    }
}
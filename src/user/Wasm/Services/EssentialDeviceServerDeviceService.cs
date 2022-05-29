using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class EssentialDeviceServerDeviceService : DeviceStorageService, IDeviceServerCommunication
{
    private readonly IConfiguration configuration;

    public EssentialDeviceServerDeviceService(IJSRuntime jsRuntime, IHttpClientFactory factory, IConfiguration configuration) : base(jsRuntime)
    {
        this.configuration = configuration;
        HttpClient = factory.CreateClient();
    }
    
    public HttpClient HttpClient { get; }
    public string GetFullAddress(string relative)
    {
        var uri = configuration["ApiBaseAddress"];
        return uri + relative;
    }
}
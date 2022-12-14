using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.Wasm.Services;

public class EssentialDeviceServerDeviceService : DeviceStorageService, IDeviceServerCommunication
{
    private readonly IConfiguration configuration;

    public HttpClient HttpClient { get; }
    
    public EssentialDeviceServerDeviceService(IJSRuntime jsRuntime, IHttpClientFactory factory, IConfiguration configuration) : base(jsRuntime)
    {
        this.configuration = configuration;
        HttpClient = factory.CreateClient();
    }
    
    public string GetFullAddress(string relative)
    {
        var uri = configuration["ApiBaseAddress"];
        return uri + relative;
    }
}
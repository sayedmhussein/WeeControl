using System;
using System.Net.Http;
using Microsoft.JSInterop;
using WeeControl.Presentations.FunctionalService.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class EssentialUserDeviceService : UserStorageService, IEssentialUserDevice
{
    public EssentialUserDeviceService(IJSRuntime jsRuntime, IHttpClientFactory factory) : base(jsRuntime)
    {
        HttpClient = factory.CreateClient();
    }
    
    public string ServerBaseAddress
    {
        get => "https://localhost:5001/";
        set => _ = value;
    }

    public HttpClient HttpClient { get; }
    public string FullAddress(string relative)
    {
        return ServerBaseAddress + relative;
    }

    public string DeviceId => "This is device _Blazor_";
    public DateTime TimeStamp => DateTime.UtcNow;
    public double? Latitude => null;
    public double? Longitude => null;
}
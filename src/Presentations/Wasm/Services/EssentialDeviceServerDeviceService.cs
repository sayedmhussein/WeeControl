using System;
using System.Net.Http;
using Microsoft.JSInterop;
using WeeControl.Presentations.ServiceLibrary.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class EssentialDeviceServerDeviceService : DeviceStorageService, IDeviceServerCommunication
{
    public EssentialDeviceServerDeviceService(IJSRuntime jsRuntime, IHttpClientFactory factory) : base(jsRuntime)
    {
        HttpClient = factory.CreateClient();
    }
    
    public string ServerBaseAddress
    {
        get => "https://localhost:5001/";
        set => _ = value;
    }

    public HttpClient HttpClient { get; }
    public string GetFullAddress(string relative)
    {
        return ServerBaseAddress + relative;
    }
}
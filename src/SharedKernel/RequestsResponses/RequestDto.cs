using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.RequestsResponses;

public class RequestDto : IRequestDto
{
    [Obsolete]
    public static HttpContent BuildHttpContentAsJson(object dto)
    {
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }

    public string DeviceId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    protected RequestDto()
    {
        DeviceId = string.Empty;
    }

    public RequestDto(string device) : this()
    {
        DeviceId = device;
    }

    public RequestDto(string device, double? latitude, double? longitude) : this(device)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
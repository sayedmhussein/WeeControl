using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContexts.Shared
{
    public class RequestDto : ISerializable, IVerifyable, IRequestDto
    {
        public static HttpContent BuildHttpContentAsJson(object dto)
        {
            return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        }
        
        public string DeviceId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public RequestDto()
        {
        }
        
        public RequestDto(string device) : this()
        {
            DeviceId = device;
        }

        public RequestDto(string device, double latitude, double longitude) : this(device)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
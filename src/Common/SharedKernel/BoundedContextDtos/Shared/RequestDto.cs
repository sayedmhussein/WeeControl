using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.BoundedContextDtos.Shared
{
    public class RequestDto : ISerializable, IVerifyable, IRequestDto
    {
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
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.DtosV1.Common
{
    public class RequestDto : ISerializable, IVerifyable, IRequestDto
    {
        public string DeviceId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
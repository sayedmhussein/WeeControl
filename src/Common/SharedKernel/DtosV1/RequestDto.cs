using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.SharedKernel.DtosV1
{
    public class RequestDto<T> : ISerializable, IVerifyable, IRequestDto<T> where T : class
    {
        public T Payload { get; set; }

        public string DeviceId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}

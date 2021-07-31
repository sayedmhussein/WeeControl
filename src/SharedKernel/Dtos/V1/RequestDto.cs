using System;
namespace WeeControl.SharedKernel.Dtos.V1
{
    public class RequestDto<T> where T : class
    {
        public T Payload { get; set; }

        public string DeviceId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}

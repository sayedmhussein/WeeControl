using System;
namespace WeeControl.SharedKernel.Dtos.V1
{
    public class ResponseDto<T> where T : class
    {
        public ResponseDto(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; set; }
    }
}

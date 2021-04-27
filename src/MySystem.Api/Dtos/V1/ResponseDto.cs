using System;
namespace MySystem.Api.Dtos.V1
{
    public class ResponseDto<T>
    {
        public T Payload { get; set; }

        public string Error { get; set; }

        public ResponseDto()
        {
        }

        public ResponseDto(T payload)
        {
            Payload = payload;
        }
    }
}

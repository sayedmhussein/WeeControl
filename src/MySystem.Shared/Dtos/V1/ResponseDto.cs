using System;

namespace Sayed.MySystem.Shared.Dtos.V1
{
    public class ResponseDto<T> : IResponseDto<T>
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

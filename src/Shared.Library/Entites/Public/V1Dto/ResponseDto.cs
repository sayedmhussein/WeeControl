using System;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Public.V1Dto
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

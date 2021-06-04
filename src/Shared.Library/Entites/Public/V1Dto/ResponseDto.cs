using System;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Public.V1Dto
{
    public class ResponseDto : IDto
    {
        public string Error { get; set; }
    }

    public class ResponseDto<T> : ResponseDto, IPayload<T>
    {
        public T Payload { get; set; }

        public ResponseDto()
        {
        }

        public ResponseDto(T payload)
        {
            Payload = payload;
        }
    }
}

using System;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Entities.Public.V1Dto
{
    [Obsolete]
    public class ResponseDto : IDto
    {
        public string Error { get; set; }
    }

    [Obsolete]
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

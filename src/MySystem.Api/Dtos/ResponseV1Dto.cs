using System;
namespace MySystem.Api.Dtos
{
    public class ResponseV1Dto<T>
    {
        public T Payload { get; set; }

        public string DebugMsg { get; set; }

        public ResponseV1Dto()
        {
        }

        public ResponseV1Dto(T payload)
        {
            Payload = payload;
        }
    }
}

using System;

namespace MySystem.Data.V1.Dtos
{
    public class ResponseV1<T>
    {
        public T Payload { get; set; }

        public string DebugMsg { get; set; }

        public ResponseV1()
        {
        }

        public ResponseV1(T payload)
        {
            Payload = payload;
        }
    }
}

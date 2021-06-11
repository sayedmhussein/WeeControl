using System;
namespace MySystem.SharedKernel.Interfaces
{
    public interface IPayload<T> : IRequestDto
    {
        T Payload { get; set; }
    }
}

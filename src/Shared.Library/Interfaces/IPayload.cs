using System;
namespace MySystem.SharedKernel.Interfaces
{
    public interface IPayload<T> : IDto
    {
        T Payload { get; set; }
    }
}

using System;
namespace MySystem.SharedKernel.Interfaces
{
    public interface IResponseDto<T> : IDto
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

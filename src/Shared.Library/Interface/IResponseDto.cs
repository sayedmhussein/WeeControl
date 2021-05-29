using System;
namespace MySystem.SharedKernel.Interface
{
    public interface IResponseDto<T>
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

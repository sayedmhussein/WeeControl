using System;
namespace MySystem.Shared.Library.Dtos
{
    public interface IResponseDto<T>
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

using System;
namespace MySystem.Shared.Library.Dto
{
    public interface IResponseDto<T>
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

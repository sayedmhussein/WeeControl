using System;
namespace Sayed.MySystem.Shared.Dtos
{
    public interface IResponseDto<T>
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

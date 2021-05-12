using System;
namespace Sayed.MySystem.SharedDto.Interfaces
{
    public interface IResponseDto<T>
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

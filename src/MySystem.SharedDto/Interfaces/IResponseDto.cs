using System;
namespace MySystem.SharedDto.Interfaces
{
    public interface IResponseDto<T>
    {
        T Payload { get; set; }

        string Error { get; set; }
    }
}

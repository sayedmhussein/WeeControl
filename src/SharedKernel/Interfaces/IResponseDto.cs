using System.Net;

namespace WeeControl.SharedKernel.Interfaces;

public interface IResponseDto
{
    bool IsSuccess { get; }

    HttpStatusCode HttpStatusCode { get; }
}
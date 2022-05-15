using System.Net;

namespace WeeControl.SharedKernel.Interfaces;

public interface IResponseDto
{
    bool IsSuccess { get; }

    string MessageToUser { get; }

    HttpStatusCode HttpStatusCode { get; }
}
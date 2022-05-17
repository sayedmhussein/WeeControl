using System.Net;

namespace WeeControl.SharedKernel.Interfaces;

public interface IResponseDto
{
    bool IsSuccess { get; }

    [Obsolete("No need to expose this to frontend, service can view the message")]
    string MessageToUser { get; }

    HttpStatusCode HttpStatusCode { get; }
}
using System.Net;

namespace WeeControl.Common.SharedKernel.Interfaces;

public interface IResponseDto
{
    bool IsSuccess { get; }

    string MessageToUser { get; }

    HttpStatusCode HttpStatusCode { get; }
}
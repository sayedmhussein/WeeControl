using System.Net;

namespace WeeControl.Common.FunctionalService.Interfaces;

public interface IResponseToUi
{
    bool IsSuccess { get; }

    string MessageToUser { get; }

    HttpStatusCode HttpStatusCode { get; }
}
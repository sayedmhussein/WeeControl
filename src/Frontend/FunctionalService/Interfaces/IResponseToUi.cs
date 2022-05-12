using System.Net;

namespace WeeControl.Frontend.FunctionalService.Interfaces;

public interface IResponseToUi
{
    bool IsSuccess { get; }

    string MessageToUser { get; }

    HttpStatusCode HttpStatusCode { get; }
}
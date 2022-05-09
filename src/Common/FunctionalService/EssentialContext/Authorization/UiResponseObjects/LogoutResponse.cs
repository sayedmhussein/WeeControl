using System.Net;
using WeeControl.Common.FunctionalService.Interfaces;

namespace WeeControl.Common.FunctionalService.EssentialContext.Authorization.UiResponseObjects;

public class LogoutResponse : IResponseToUi
{
    public bool IsSuccess { get; }
    public string MessageToUser { get; }
    public HttpStatusCode HttpStatusCode { get; }
}
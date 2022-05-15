using MediatR;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class LogoutCommand : IRequest
{
    public LogoutCommand(RequestDto request)
    {
        Request = request;
    }

    public RequestDto Request { get; }
}
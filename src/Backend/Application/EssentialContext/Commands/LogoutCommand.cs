using System;
using MediatR;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Backend.Application.EssentialContext.Commands;

public class LogoutCommand : IRequest
{
    public LogoutCommand(RequestDto request)
    {
        Request = request;
    }

    public RequestDto Request { get; }
}
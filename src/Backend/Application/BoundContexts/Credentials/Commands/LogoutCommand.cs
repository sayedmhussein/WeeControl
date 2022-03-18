using System;
using MediatR;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Commands
{
    public class LogoutCommand : IRequest
    {
        public LogoutCommand(RequestDto request, Guid? sessionid)
        {
            Request = request;
            Sessionid = sessionid;
        }

        public RequestDto Request { get; }
        public Guid? Sessionid { get; }
    }
}

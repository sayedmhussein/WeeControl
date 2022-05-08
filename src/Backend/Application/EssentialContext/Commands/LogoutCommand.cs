﻿using System;
using MediatR;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Backend.Application.EssentialContext.Commands
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
﻿using MediatR;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Commands
{
    public class RegisterCommand : IRequest<TokenDto>
    {
        public RegisterCommand(RequestDto request, RegisterDto payload)
        {
            Request = request;
            Payload = payload;
        }

        public RequestDto Request { get; }
        public RegisterDto Payload { get; }
    }
}
using System;
using MediatR;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Backend.Application.BoundContexts.Credentials.Queries
{
    public class RegisterCommand : IRequest<ResponseDto<LoginDto>>
    {
        public RegisterCommand()
        {
        }
    }
}

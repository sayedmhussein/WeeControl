using System;
using MediatR;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.LogoutEmployee
{
    public class LogoutEmployeeCommand : IRequest<ResponseDto>
    {
        public string Device { get; set; }

        public Guid? SessionId { get; set; }

        public LogoutEmployeeCommand(string device, Guid? sessionId)
        {
            Device = device;
            SessionId = sessionId;
        }
    }
}
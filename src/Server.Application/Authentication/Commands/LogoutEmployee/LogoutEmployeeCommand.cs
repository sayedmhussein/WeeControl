using System;
using MediatR;
using WeeControl.SharedKernel.Common.DtosV1;

namespace WeeControl.Application.HumanResources.Commands.LogoutEmployee
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
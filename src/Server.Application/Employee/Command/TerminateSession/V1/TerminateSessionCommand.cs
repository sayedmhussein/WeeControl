using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Entities.Employee.V1Dto;

namespace MySystem.Application.Employee.Command.TerminateSession.V1
{
    public class TerminateSessionCommand : RequestDto<LogoutDto>, IRequest
    {
    }
}

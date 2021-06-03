using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.V1Dto;

namespace MySystem.Application.Employee.Command.AddEmployeeSession.V1
{
    public class AddEmployeeSessionCommand : RequestDto<LoginDto>, IRequest<IEnumerable<Claim>>
    {
    }
}

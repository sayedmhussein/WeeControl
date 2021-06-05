using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Entities.Employee.V1Dto;

namespace Application.Employee.Command.AddEmployeeClaim.V1
{
    public class AddEmployeeClaimCommand : RequestDto<EmployeeClaimDto>, IRequest<ResponseDto<EmployeeClaimDto>>
    {
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Entities.Employee.V1Dto;

namespace Application.Employee.Command.UpdateEmployee.V1
{
    public class UpdateEmployeeCommand : EmployeeDto, IRequest<ResponseDto<EmployeeDto>>
    {
    }
}

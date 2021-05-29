using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.ExtensionMethod;

namespace MySystem.Application.Employee.Command.CreateEmployee.V1
{
    public class CreateEmployeeCommand : RequestDto<EmployeeDto>, IRequest<ResponseDto<EmployeeDto>>
    {
    }
}

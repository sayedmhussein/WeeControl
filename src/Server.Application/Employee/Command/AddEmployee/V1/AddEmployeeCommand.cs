using MediatR;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Entities.Employee.V1Dto;

namespace MySystem.Application.Employee.Command.AddEmployee.V1
{
    public class AddEmployeeCommand : RequestDto<EmployeeDto>, IRequest<ResponseDto<EmployeeDto>>
    {
    }
}

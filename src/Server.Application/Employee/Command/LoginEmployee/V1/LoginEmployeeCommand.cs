using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.LoginEmployee.V1
{
    public class LoginEmployeeCommand : RequestDto<LoginDto>, IRequest<ResponseDto<string>>
    {
    }
}

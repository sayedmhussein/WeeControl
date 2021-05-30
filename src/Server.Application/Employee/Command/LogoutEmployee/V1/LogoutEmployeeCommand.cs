using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.LogoutEmployee.V1
{
    public class LogoutEmployeeCommand : IRequest<ResponseDto<bool>>
    {
        public string Token { get; set; }

        public Guid? EmployeeId { get; set; }
    }
}

using System;
using MediatR;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.RefreshEmployeeToken.V1
{
    public class RefreshEmployeeTokenCommand : IRequest<ResponseDto<string>>
    {
        public string Token { get; set; }
    }
}

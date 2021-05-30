using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.LogoutEmployee.V1
{
    public class LogoutEmployeeHandler : IRequestHandler<LogoutEmployeeCommand, ResponseDto<bool>>
    {
        private readonly IMySystemDbContext context;
        private readonly IJwtService jwtService;

        public LogoutEmployeeHandler(IMySystemDbContext context, IJwtService jwtService)
        {
            this.context = context;
            this.jwtService = jwtService;
        }

        public Task<ResponseDto<bool>> Handle(LogoutEmployeeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

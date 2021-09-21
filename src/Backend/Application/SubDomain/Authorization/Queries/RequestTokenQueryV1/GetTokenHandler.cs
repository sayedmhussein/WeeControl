using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Application.SubDomain.Employee.Queries.GetClaimsV1;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Domain.EntityGroups.Employee;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
using WeeControl.Common.SharedKernel.DataTransferObjectV1.Employee;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.SubDomain.Authorization.Queries.RequestTokenQueryV1
{
    public class GetTokenHandler : IRequestHandler<GetTokenQuery, ResponseDto<EmployeeTokenDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;

        public GetTokenHandler(IMySystemDbContext context, IJwtService jwtService, IMediator mediator)
        {
            this.context = context;
            this.jwtService = jwtService;
            this.mediator = mediator;
        }
        
        public async Task<ResponseDto<EmployeeTokenDto>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            EmployeeTokenDto tokenDto;
            if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Password))
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x =>
                    x.Username == request.Username && 
                    x.Password == request.Password, cancellationToken);

                VerifyEmployee(employee);
                //var token = BuildToken(claims, DateTime.UtcNow.AddMinutes(5));
                
                throw new System.NotImplementedException();
            }
            else if (request.SessionId is not null)
            {
                throw new System.NotImplementedException();
            }
            else
            {
                throw new BadRequestException("Didn't provide correct dto.");
            }
        }
        
        private string BuildToken(IEnumerable<Claim> claims, DateTime validity)
        {
            var token = jwtService.GenerateJwtToken(claims, "WeeControl", validity);
            return token;
        }

        private void VerifyEmployee(EmployeeDbo employee)
        {
            if (employee == null)
            {
                throw new NotFoundException("Username or password are not matching.");
            }

            if (string.IsNullOrWhiteSpace(employee.AccountLockArgument) == false)
            {
                throw new NotAllowedException("User account is locked.");
            }
        }
    }
}
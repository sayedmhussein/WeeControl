using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken
{
    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<EmployeeTokenDto>>
    {
        private readonly IHumanResourcesDbContext context;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;

        public GetNewTokenHandler(IHumanResourcesDbContext context, IJwtService jwtService, IMediator mediator)
        {
            this.context = context;
            this.jwtService= jwtService;
            this.mediator = mediator;
        }
        
        public async Task<ResponseDto<EmployeeTokenDto>> Handle(GetNewTokenQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Device))
            {
                throw new BadRequestException("Didn't provide device id.");
            }
            
            if (!string.IsNullOrWhiteSpace(request.Username) && !string.IsNullOrWhiteSpace(request.Password))
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => 
                    x.Credentials.Username == request.Username && 
                    x.Credentials.Password == request.Password, cancellationToken);

                if (employee is null) throw new NotFoundException();
                
                var descriptor = new SecurityTokenDescriptor()
                {
                    IssuedAt = DateTime.UtcNow
                };
                var token = jwtService.GenerateToken(descriptor);

                return new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto()
                {
                    Token = token,
                    FullName = employee.EmployeeName, PhotoUrl = "url"
                }) { HttpStatuesCode = HttpStatusCode.OK};
            }
            else if (request.SessionId is not null)
            {
                throw new System.NotImplementedException();
            }
            else
            {
                throw new BadRequestException("Didn't valid request query parameters.");
            }
        }
    
        private string BuildToken(IEnumerable<Claim> claims, DateTime validity)
        {
            var descriptor = new SecurityTokenDescriptor()
            {
            };
            var token = jwtService.GenerateToken(descriptor);
            return token;
        }
    }
}
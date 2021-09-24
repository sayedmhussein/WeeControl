using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using WeeControl.Common.UserSecurityLib;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Queries.GetNewToken
{
    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<EmployeeTokenDto>>
    {
        private readonly IHumanResourcesDbContext context;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public GetNewTokenHandler(IHumanResourcesDbContext context, IJwtService jwtService, IMediator mediator, IConfiguration configuration)
        {
            this.context = context;
            this.jwtService= jwtService;
            this.mediator = mediator;
            this.configuration = configuration;
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
                
                var session = Session.Create(employee.EmployeeId, request.Device);
                await context.Sessions.AddAsync(session, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(SecurityClaims.HumanResources.Session, session.SessionId.ToString()));

                //var temp = new ClaimsIdentity(new[] {new Claim("sss", session.SessionId.ToString())});

                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
                
                var descriptor = new SecurityTokenDescriptor()
                {
                    Subject = ci,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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
                var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId && x.TerminationTs == null, cancellationToken);
                if (session is null) throw new NotAllowedException("Please login again.");
                
                session.Logs.Add(new SessionLog("Verified."));
                await context.SaveChangesAsync(cancellationToken);

                var employee = await 
                    context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == session.EmployeeId, cancellationToken);
                
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
            else
            {
                throw new BadRequestException("Didn't valid request query parameters.");
            }
        }
        
    }
}
using System;
using System.Linq;
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
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;
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

                var session = await context.Sessions.FirstOrDefaultAsync(x => x.EmployeeId == employee.EmployeeId && x.TerminationTs == null, cancellationToken);
                if (session is null)
                {
                    session = Session.Create(employee.EmployeeId, request.Device);
                    await context.Sessions.AddAsync(session, cancellationToken);
                }
                else
                {
                    if (session.DeviceId != request.Device)
                    {
                        session.TerminationTs = DateTime.UtcNow;
                        await context.SaveChangesAsync(cancellationToken);
                        throw new NotAllowedException("User used session not related to device!");
                    }
                }
                
                await context.SaveChangesAsync(cancellationToken);

                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(HumanResourcesData.Claims.Session, session.SessionId.ToString()));

                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
                
                var descriptor = new SecurityTokenDescriptor()
                {
                    Subject = ci,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = jwtService.GenerateToken(descriptor);
                
                return new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto(token, employee.EmployeeName, "url")) { StatuesCode = HttpStatusCode.OK};
            }
            else if (request.SessionId is not null)
            {
                var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId && x.TerminationTs == null && x.DeviceId == request.Device, cancellationToken);
                if (session is null) throw new NotAllowedException("Please login again.");
                
                session.Logs.Add(new SessionLog("Verified."));
                await context.SaveChangesAsync(cancellationToken);

                var employee = await 
                    context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == session.EmployeeId, cancellationToken);

                
                
                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(HumanResourcesData.Claims.Session, session.SessionId.ToString()));
                ci.AddClaim(new Claim(HumanResourcesData.Claims.Territory, employee.TerritoryCode));
                foreach (var c in employee.Claims.Where(x => x.RevokedTs == null).ToList())
                {
                    ci.AddClaim(new Claim(c.ClaimType, c.ClaimValue));
                }
                
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
                
                var descriptor = new SecurityTokenDescriptor()
                {
                    Subject = ci,
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddDays(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = jwtService.GenerateToken(descriptor);

                return new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto()
                {
                    Token = token,
                    FullName = employee.EmployeeName, PhotoUrl = "url"
                }) { StatuesCode = HttpStatusCode.OK};
            }
            else
            {
                throw new BadRequestException("Invalid request query parameters.");
            }
        }
        
    }
}
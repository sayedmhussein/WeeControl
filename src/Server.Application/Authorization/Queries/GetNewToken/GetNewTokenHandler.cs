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
using WeeControl.Application.Common.Exceptions;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.Authorization.Entities;
using WeeControl.Server.Domain.Authorization.ValueObjects;
using WeeControl.SharedKernel.Authorization.DtosV1;
using WeeControl.SharedKernel.Common.DtosV1;
using WeeControl.SharedKernel.Common.Interfaces;
using WeeControl.SharedKernel.Security.BoundedContexts.HumanResources;

namespace WeeControl.Application.Authorization.Queries.GetNewToken
{
    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<EmployeeTokenDto>>
    {
        private readonly IAuthorizationDbContext context;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public GetNewTokenHandler(IAuthorizationDbContext context, IJwtService jwtService, IMediator mediator, IConfiguration configuration)
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
            
            if (request.SessionId is not null)
            {
                var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId && x.TerminationTs == null && x.DeviceId == request.Device, cancellationToken);
                if (session is null) throw new NotAllowedException("Please login again.");
                
                session.Logs.Add(new SessionLog("Verified."));
                await context.SaveChangesAsync(cancellationToken);

                var user = await 
                    context.Users.Include(x => x.Claims).FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);

                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(HumanResourcesData.Claims.Session, session.SessionId.ToString()));
                //ci.AddClaim(new Claim(HumanResourcesData.Claims.Territory, user.TerritoryCode));
                
                foreach (var c in user.Claims.Where(x => x.RevokedTs == null).ToList())
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
                    FullName = user.Username, PhotoUrl = "url"
                }) { StatuesCode = HttpStatusCode.OK};
            }

            if (string.IsNullOrWhiteSpace(request.RequestNewTokenDto.Password))
                throw new BadRequestException("Invalid request query parameters.");
            {
                User user;
                if (!string.IsNullOrWhiteSpace(request.RequestNewTokenDto.Username))
                {
                    user = await context.Users.FirstOrDefaultAsync(x => 
                        x.Username == request.RequestNewTokenDto.Username && 
                        x.Password == request.RequestNewTokenDto.Password, cancellationToken);
                }
                else if (!string.IsNullOrWhiteSpace(request.RequestNewTokenDto.UserEmail))
                {
                    user = await context.Users.FirstOrDefaultAsync(x => 
                        x.Username == request.RequestNewTokenDto.UserEmail && 
                        x.Password == request.RequestNewTokenDto.Password, cancellationToken);
                }
                else if (!string.IsNullOrWhiteSpace(request.RequestNewTokenDto.UserMobile))
                {
                    user = await context.Users.FirstOrDefaultAsync(x => 
                        x.Username == request.RequestNewTokenDto.UserMobile && 
                        x.Password == request.RequestNewTokenDto.Password, cancellationToken);
                }
                else
                {
                    throw new BadRequestException("Neither username nor email nor mobile was provided!");
                }
                

                if (user is null) throw new NotFoundException();

                var session = await context.Sessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.TerminationTs == null, cancellationToken);
                if (session is null)
                {
                    session = new UserSession(user.UserId, request.Device);
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
                
                return new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto(token, user.Username, "url")) { StatuesCode = HttpStatusCode.OK};
            }

        }
        
    }
}
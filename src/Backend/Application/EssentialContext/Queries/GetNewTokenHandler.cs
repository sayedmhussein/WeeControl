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
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Domain.Databases.Essential;
using WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;
using WeeControl.Common.SharedKernel.DataTransferObjects.Essential.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.Application.EssentialContext.Queries
{
    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<TokenDto>>
    {
        private readonly IEssentialDbContext context;
        private readonly IJwtService jwtService;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly ICurrentUserInfo currentUserInfo;

        public GetNewTokenHandler(
            IEssentialDbContext context, 
            IJwtService jwtService, 
            IMediator mediator, 
            IConfiguration configuration,
            ICurrentUserInfo currentUserInfo)
        {
            this.context = context;
            this.jwtService = jwtService;
            this.mediator = mediator;
            this.configuration = configuration;
            this.currentUserInfo = currentUserInfo;
        }

        public async Task<ResponseDto<TokenDto>> Handle(GetNewTokenQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Payload?.UsernameOrEmail) && !string.IsNullOrWhiteSpace(request.Payload?.Password))
            {
                var employee = await context.Users.FirstOrDefaultAsync(x =>
                    (x.Username == request.Payload.UsernameOrEmail || x.Email == request.Payload.UsernameOrEmail) &&
                    x.Password == request.Payload.Password, cancellationToken);

                if (employee is null) throw new NotFoundException();

                var session = await context.Sessions.FirstOrDefaultAsync(x => x.UserId == employee.UserId && x.TerminationTs == null, cancellationToken);
                if (session is null)
                {
                    session = new SessionDbo() { UserId = employee.UserId, DeviceId = request.Request.DeviceId};
                    await context.Sessions.AddAsync(session, cancellationToken);
                }
                else
                {
                    if (session.DeviceId != request.Request.DeviceId)
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

                return new ResponseDto<TokenDto>(new TokenDto(token, employee.Username, "url"));
            }
            else if (currentUserInfo.GetSessionId() is not null)
            {
                var session = await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == currentUserInfo.GetSessionId() && x.TerminationTs == null && x.DeviceId == request.Request.DeviceId, cancellationToken);
                if (session is null) throw new NotAllowedException("Please login again.");

                //session.Logs.Add(new SessionLog("Verified."));
                await context.SaveChangesAsync(cancellationToken);

                var employee = await
                    context.Users.Include(x => x.Claims).FirstOrDefaultAsync(x => x.UserId == session.UserId, cancellationToken);



                var ci = new ClaimsIdentity("custom");
                ci.AddClaim(new Claim(HumanResourcesData.Claims.Session, session.SessionId.ToString()));
                ci.AddClaim(new Claim(HumanResourcesData.Claims.Territory, employee.TerritoryCode));
                //foreach (var c in employee.Claims.Where(x => x.RevokedTs == null).ToList())
                foreach (var c in employee.Claims?.Where(x => x.RevokedTs == null)?.ToList())
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

                return new ResponseDto<TokenDto>(new TokenDto()
                {
                    Token = token,
                    FullName = employee.UserId.ToString(),
                    PhotoUrl = "url"
                });
            }
            else
            {
                throw new BadRequestException("Invalid request query parameters.");
            }
        }
    }
}

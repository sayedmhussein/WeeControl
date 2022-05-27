using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Queries;

public class GetUserDetailsQuery : IRequest<ResponseDto<UserDetailedDto>>
{
    private string Username { get; }

    public GetUserDetailsQuery(string username)
    {
        Username = username;
    }
    
    public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsQuery, ResponseDto<UserDetailedDto>>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo userInfo;

        public GetUserDetailsHandler(IEssentialDbContext context, ICurrentUserInfo userInfo)
        {
            this.context = context;
            this.userInfo = userInfo;
        }
        
        public async Task<ResponseDto<UserDetailedDto>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var userDbo = await context.Users
                .Include(c => c.Claims)
                .ThenInclude(x => x.GrantedBy)
                .Include(c => c.Claims)
                .ThenInclude(x => x.RevokedBy)
                .Include(s => s.Sessions.Where(x => x.TerminationTs == null))
                .ThenInclude(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (userDbo is null)
            {
                throw new NotFoundException();
            }

            var claims = new List<ClaimDto>();
            userDbo.Claims.ToList().ForEach(c =>
            {
                claims.Add(new ClaimDto()
                {
                    ClaimType = c.ClaimType,
                    ClaimValue = c.ClaimValue,
                    GrantedTs = c.GrantedTs,
                    RevokedTs = c.RevokedTs,
                    GrantedByUsername = c.GrantedBy?.Username,
                    RevokedByUsername = c.RevokedBy?.Username
                });
            });

            var sessions = userDbo.Sessions.Select(c =>
                {
                    var logs = new List<SessionLogDto>();
                    foreach (var log in c.Logs)
                    {
                        logs.Add(new SessionLogDto(){ Context = log.Context, Details = log.Details, LogTs = log.LogTs});
                    }
                    return new SessionDto()
                    {
                        SessionId = c.SessionId,
                        DeviceId = c.DeviceId,
                        CreatedTs = c.CreatedTs,
                        TerminationTs = c.TerminationTs,
                        Logs = logs
                    };
                })
                .ToList();

            var user = new UserDetailedDto()
            {
                Email = userDbo.Email, Username = userDbo.Username, TerritoryCode = userDbo.TerritoryId, 
                Claims = claims,
                Sessions = sessions
            };
            
            return new ResponseDto<UserDetailedDto>(user);
        }
    }
}
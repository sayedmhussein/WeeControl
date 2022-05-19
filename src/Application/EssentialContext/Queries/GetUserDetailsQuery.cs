using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Essential.ResponseDTOs;
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
                .Include(s => s.Sessions.Where(x => x.TerminationTs == null))
                .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            if (userDbo is null)
            {
                throw new NotFoundException();
            }

            var user = new UserDetailedDto()
            {
                Email = userDbo.Email, Username = userDbo.Username, TerritoryCode = userDbo.TerritoryCode, 
                Temp = "Count of Sessions Are: " + userDbo.Sessions.Count()
            };
            
            return new ResponseDto<UserDetailedDto>(user);
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Queries;

public class GetListOfUsersQuery : IRequest<ResponseDto<IEnumerable<UserDto>>>
{
    
    
    public class GetListOfUsersHandler : IRequestHandler<GetListOfUsersQuery, ResponseDto<IEnumerable<UserDto>>>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo userInfo;

        public GetListOfUsersHandler(IEssentialDbContext context, ICurrentUserInfo userInfo)
        {
            this.context = context;
            this.userInfo = userInfo;
        }
    
        public async Task<ResponseDto<IEnumerable<UserDto>>> Handle(GetListOfUsersQuery request, CancellationToken cancellationToken)
        {
            await userInfo.LogUserActivityAsync("Essential", "Getting List of Users", cancellationToken);
            var users = new List<UserDto>();
            foreach (var userDbo in await context.Users.ToListAsync(cancellationToken))
            {
                users.Add(new UserDto()
                {
                    Email = userDbo.Email, Username = userDbo.Username, TerritoryCode = userDbo.TerritoryId
                });
            }

            var response = new ResponseDto<IEnumerable<UserDto>>(users);
            return response;
        }
    }
}
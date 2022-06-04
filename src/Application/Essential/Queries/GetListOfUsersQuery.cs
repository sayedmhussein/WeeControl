using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Essential.Queries;

public class GetListOfUsersQuery : IRequest<ResponseDto<IEnumerable<UserDtoV1>>>
{
    public class GetListOfUsersHandler : IRequestHandler<GetListOfUsersQuery, ResponseDto<IEnumerable<UserDtoV1>>>
    {
        private readonly IEssentialDbContext context;
        private readonly ICurrentUserInfo userInfo;

        public GetListOfUsersHandler(IEssentialDbContext context, ICurrentUserInfo userInfo)
        {
            this.context = context;
            this.userInfo = userInfo;
        }
    
        public async Task<ResponseDto<IEnumerable<UserDtoV1>>> Handle(GetListOfUsersQuery request, CancellationToken cancellationToken)
        {
            await userInfo.LogUserActivityAsync("Essential", "Getting List of Users", cancellationToken);
            var users = new List<UserDtoV1>();
            var allowed = await userInfo.GetTerritoriesListAsync(default);
            foreach (var userDbo in await context.Users.Where(x => allowed.Contains(x.TerritoryId)).ToListAsync(cancellationToken))
            {
                users.Add(new UserDtoV1()
                {
                    Email = userDbo.Email, Username = userDbo.Username, TerritoryCode = userDbo.TerritoryId
                });
            }

            var response = new ResponseDto<IEnumerable<UserDtoV1>>(users);
            return response;
        }
    }
}
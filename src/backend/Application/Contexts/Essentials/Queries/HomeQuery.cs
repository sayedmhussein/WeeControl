using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Queries;

public class HomeQuery : IRequest<ResponseDto<HomeResponseDto>>
{
    public class HomeHandler : IRequestHandler<HomeQuery, ResponseDto<HomeResponseDto>>
    {
        private readonly IEssentialDbContext essentialDbContext;
        private readonly ICurrentUserInfo userInfo;

        public HomeHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo userInfo)
        {
            this.essentialDbContext = essentialDbContext;
            this.userInfo = userInfo;
        }

        public async Task<ResponseDto<HomeResponseDto>> Handle(HomeQuery request, CancellationToken cancellationToken)
        {
            var user = await essentialDbContext.UserSessions
                .Where(x => x.SessionId == userInfo.SessionId)
                .Where(x => x.TerminationTs == null)
                .Include(x => x.User)
                .ThenInclude(x => x.Person)
                .Where(x => x.User.SuspendArgs == null)
                .Select(x => new
                {
                    x.User.UserId,
                    x.User.Person.FirstName, x.User.Person.LastName,
                    x.User.PhotoUrl
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null) throw new NotFoundException("No valid session was found");

            var notifications = await essentialDbContext.UserNotifications
                .Where(x => x.UserId == user.UserId)
                .ToListAsync(cancellationToken);

            var feeds = await essentialDbContext.Feeds
                .Where(x => x.HideTs == null)
                .ToListAsync(cancellationToken);

            var dto = ResponseDto.Create(new HomeResponseDto
            {
                FullName = user.FirstName + " " + user.LastName,
                PhotoUrl = user.PhotoUrl,
                Notifications = notifications,
                Feeds = feeds
            });

            return dto;
        }
    }
}
using System;
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

public class NotificationQuery : IRequest<ResponseDto<IEnumerable<NotificationDto>>>
{
    public class NotificationHandler : IRequestHandler<NotificationQuery, ResponseDto<IEnumerable<NotificationDto>>>
    {
        private readonly IEssentialDbContext essentialDbContext;
        private readonly ICurrentUserInfo userInfo;

        public NotificationHandler(IEssentialDbContext essentialDbContext, ICurrentUserInfo userInfo)
        {
            this.essentialDbContext = essentialDbContext;
            this.userInfo = userInfo;
        }
        
        public async Task<ResponseDto<IEnumerable<NotificationDto>>> Handle(NotificationQuery request, CancellationToken cancellationToken)
        {
            var user = await 
                essentialDbContext.Sessions.FirstOrDefaultAsync(x => x.SessionId == userInfo.GetSessionId(),
                    cancellationToken);
            if (user is null)
            {
                throw new ArgumentNullException();
            }

            var _list = new List<NotificationDto>();

            var list = essentialDbContext.UserNotifications
                .Where(x => x.UserId == user.UserId && x.ViewedTs == null)
                .ToList();
            foreach (var dbo in list)
            {
                _list.Add(new NotificationDto()
                {
                    NotificationId = dbo.NotificationId,
                    Subject = dbo.Subject,
                    Details = dbo.Details,
                    Link = dbo.Link
                });
            }

            return ResponseDto.Create<IEnumerable<NotificationDto>>(_list);
        }
    }
}
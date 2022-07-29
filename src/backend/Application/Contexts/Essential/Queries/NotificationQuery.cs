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

namespace WeeControl.Application.Contexts.Essential.Queries;

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
                essentialDbContext.UserSessions.FirstOrDefaultAsync(
                    x => x.SessionId == userInfo.SessionId,
                    cancellationToken);
            
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var listDbo = essentialDbContext.UserNotifications
                .Where(x => x.UserId == user.UserId && x.ReadTs == null)
                ;//.ToList();
            
            var listDto = listDbo.Select(dbo => new NotificationDto()
            {
                NotificationId = dbo.NotificationId, 
                Subject = dbo.Subject, 
                Details = dbo.Details, 
                Link = dbo.Link,
                ReadTs = dbo.ReadTs
            }).ToList();

            return ResponseDto.Create<IEnumerable<NotificationDto>>(listDto);
        }
    }
}
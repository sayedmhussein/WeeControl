using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Queries;

public class GetUserIdAndSessionVerificationQuery : IRequest<Guid>
{
    public class UserIdVerificationHandler : IRequestHandler<GetUserIdAndSessionVerificationQuery, Guid>
    {
        private readonly ICurrentUserInfo currentUserInfo;
        private readonly IEssentialDbContext dbContext;

        public UserIdVerificationHandler(IEssentialDbContext dbContext, ICurrentUserInfo currentUserInfo)
        {
            this.currentUserInfo = currentUserInfo;
            this.dbContext = dbContext;
        }

        public async Task<Guid> Handle(GetUserIdAndSessionVerificationQuery request,
            CancellationToken cancellationToken)
        {
            if (currentUserInfo.SessionId is null) throw new NotAllowedException("Session Isn't found");

            var user = await dbContext.UserSessions
                .Where(x => x.SessionId == currentUserInfo.SessionId)
                .Where(x => x.TerminationTs == null)
                .Include(x => x.User)
                .Where(x => x.User.SuspendArgs == null)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == Guid.Empty) throw new NotFoundException("Session is either terminated or user is locked");

            return user;
        }
    }
}
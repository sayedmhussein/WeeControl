using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.Contexts.Essential.Queries;

public class UserDuplicationQuery : IRequest
{
    private readonly string username;
    private readonly string email;
    private readonly string mobile;

    public UserDuplicationQuery(string username = null, string email = null, string mobile = null)
    {
        this.username = username;
        this.email = email;
        this.mobile = mobile;
    }
    
    public class UserDuplicationHandler : IRequestHandler<UserDuplicationQuery>
    {
        private readonly IEssentialDbContext essentialDbContext;

        public UserDuplicationHandler(IEssentialDbContext essentialDbContext)
        {
            this.essentialDbContext = essentialDbContext;
        }
        
        public async Task<Unit> Handle(UserDuplicationQuery request, CancellationToken cancellationToken)
        {
            if (request.username is not null &&
                await essentialDbContext.Users.Select(x => x.Username)
                    .ContainsAsync(request.username.Trim().ToLower(), cancellationToken))
            {
                throw new ConflictFailureException("username already exist");
            }

            if (request.email is not null &&
                await essentialDbContext.Users.Select(x => x.Email)
                    .ContainsAsync(request.email.Trim().ToLower(), cancellationToken))
            {
                throw new ConflictFailureException("username already exist");
            }
            
            if (request.mobile is not null &&
                await essentialDbContext.Users.Select(x => x.MobileNo)
                    .ContainsAsync(request.mobile.Trim().ToLower(), cancellationToken))
            {
                throw new ConflictFailureException("username already exist");
            }

            return Unit.Value;
        }
    }
}
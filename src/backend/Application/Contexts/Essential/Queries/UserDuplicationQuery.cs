using System;
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
    public enum Parameter
    {
        Username, Email, Mobile
    };

    private readonly Parameter parameter;
    private readonly string value;

    public UserDuplicationQuery(Parameter parameter, string value)
    {
        this.parameter = parameter;
        this.value = value;
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
            switch (request.parameter)
            {
                case Parameter.Username:
                    if (request.value is not null &&
                        await essentialDbContext.Users.Select(x => x.Username)
                            .ContainsAsync(request.value.Trim().ToLower(), cancellationToken))
                    {
                        throw new ConflictFailureException("username already exist");
                    }
                    break;
                case Parameter.Email:
                    if (request.value is not null &&
                        await essentialDbContext.Users.Select(x => x.Email)
                            .ContainsAsync(request.value.Trim().ToLower(), cancellationToken))
                    {
                        throw new ConflictFailureException("username already exist");
                    }
                    break;
                case Parameter.Mobile:
                    if (request.value is not null &&
                        await essentialDbContext.Users.Select(x => x.MobileNo)
                            .ContainsAsync(request.value.Trim().ToLower(), cancellationToken))
                    {
                        throw new ConflictFailureException("username already exist");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Unit.Value;
        }
    }
}
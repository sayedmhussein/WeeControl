using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.Contexts.Temporary.User;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.User.Queries;

public class UserDuplicationQuery : IRequest
{
    public enum Parameter
    {
        Username, Email, Mobile
    };

    private readonly string parameter;
    private readonly string value;

    public UserDuplicationQuery(string parameter, string value)
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
                case nameof(EmployeeRegisterDto.User.Username):
                    if (request.value is not null &&
                        await essentialDbContext.Users.Select(x => x.Username.Trim().ToLower())
                            .ContainsAsync(request.value.Trim().ToLower(), cancellationToken))
                    {
                        throw new ConflictFailureException("username already exist");
                    }
                    break;
                case nameof(EmployeeRegisterDto.User.Email):
                    if (request.value is not null &&
                        await essentialDbContext.Users.Select(x => x.Email.Trim().ToLower())
                            .ContainsAsync(request.value.Trim().ToLower(), cancellationToken))
                    {
                        throw new ConflictFailureException("username already exist");
                    }
                    break;
                case nameof(EmployeeRegisterDto.User.MobileNo):
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
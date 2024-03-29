using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Queries;

public class UserDuplicationQuery : IRequest
{
    public enum Parameter
    {
        Username,
        Email,
        Mobile
    }

    private readonly Parameter parameter;
    private readonly string value;

    public UserDuplicationQuery(Parameter parameter, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BadRequestException("Must provide value");

        this.parameter = parameter;
        this.value = value.Trim();
    }

    public class UserDuplicationHandler : IRequestHandler<UserDuplicationQuery>
    {
        private readonly IEssentialDbContext essentialDbContext;

        public UserDuplicationHandler(IEssentialDbContext essentialDbContext)
        {
            this.essentialDbContext = essentialDbContext;
        }

        public async Task Handle(UserDuplicationQuery request, CancellationToken cancellationToken)
        {
            switch (request.parameter)
            {
                case Parameter.Username:
                    if (await essentialDbContext.Users.Select(x => x.Username)
                            .ContainsAsync(request.value.ToLower(), cancellationToken))
                        throw new ConflictFailureException("username already exist");
                    break;
                case Parameter.Email:
                    if (await essentialDbContext.Users.Select(x => x.Email)
                            .ContainsAsync(request.value.ToLower(), cancellationToken))
                        throw new ConflictFailureException("email already exist");
                    break;
                case Parameter.Mobile:
                    var bla = essentialDbContext.Users
                        .Include(x => x.Person)
                        .ThenInclude(x => x.Contacts)
                        .Select(x => x.Person.Contacts.Select(y => y.ContactValue)
                            .Contains(request.value.ToUpper()));
                    if (await bla.ContainsAsync(true, cancellationToken))
                        throw new ConflictFailureException("mobile already exist");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //return Unit.Value;
        }
    }
}
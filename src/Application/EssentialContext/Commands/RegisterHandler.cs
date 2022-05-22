using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class RegisterHandler : IRequestHandler<RegisterCommand, ResponseDto<TokenDtoV1>>
{
    private readonly IEssentialDbContext context;
    private readonly IMediator mediator;
    private readonly IPasswordSecurity passwordSecurity;

    public RegisterHandler(IEssentialDbContext context, IMediator mediator, IPasswordSecurity passwordSecurity)
    {
        this.context = context;
        this.mediator = mediator;
        this.passwordSecurity = passwordSecurity;
    }

    public async Task<ResponseDto<TokenDtoV1>> Handle(RegisterCommand cmd, CancellationToken cancellationToken)
    {
        await mediator.Send(new VerifyRequestQuery(cmd.Request), cancellationToken);

        if (string.IsNullOrWhiteSpace(cmd.Payload.Password) ||
            (string.IsNullOrWhiteSpace(cmd.Payload.Username) && string.IsNullOrWhiteSpace(cmd.Payload.Email)))
        {
            throw new ValidationException();
        }

        if (context.Users.Any(x => (x.Username == cmd.Payload.Username.ToLower()) ||
                                   ( x.Email == cmd.Payload.Email.ToLower())))
        {
            throw new ConflictFailureException();
        }

        var user = UserDbo.Create(cmd.Payload.Email.ToLower(), cmd.Payload.Username.ToLower(), passwordSecurity.Hash(cmd.Payload.Password));

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        var b= await mediator.Send(new GetNewTokenQuery(cmd.Request, new LoginDtoV1(user.Username, cmd.Payload.Password)), cancellationToken);
        return new ResponseDto<TokenDtoV1>(b.Payload);
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Application.Contexts.System.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Contexts.Essential.Commands;

public class RegisterCommand : IRequest<IResponseDto<TokenDtoV1>>
{
    private readonly IRequestDto<RegisterDtoV1> dto;

    public RegisterCommand(IRequestDto<RegisterDtoV1> dto)
    {
        this.dto = dto;
    }
    
    public class RegisterHandler : IRequestHandler<RegisterCommand, IResponseDto<TokenDtoV1>>
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

        public async Task<IResponseDto<TokenDtoV1>> Handle(RegisterCommand cmd, CancellationToken cancellationToken)
        {
            await mediator.Send(new VerifyRequestQuery(cmd.dto), cancellationToken);

            if (string.IsNullOrWhiteSpace(cmd.dto.Payload.FirstName) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.LastName) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.Email) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.Username) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.Password) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.MobileNo) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.TerritoryId)
               )
            {
                throw new ValidationException();
            }

            if (context.Users.Any(x => 
                    x.Username == cmd.dto.Payload.Username.ToLower() ||
                    x.Email == cmd.dto.Payload.Email.ToLower() ||
                    x.MobileNo == cmd.dto.Payload.MobileNo.ToLower()
                    ))
            {
                throw new ConflictFailureException();
            }

            var user = UserDbo.Create(
                cmd.dto.Payload.FirstName,
                cmd.dto.Payload.LastName,
                cmd.dto.Payload.Email.ToLower(), 
                cmd.dto.Payload.Username.ToLower(), 
                passwordSecurity.Hash(cmd.dto.Payload.Password),
                cmd.dto.Payload.MobileNo,
                cmd.dto.Payload.TerritoryId,
                cmd.dto.Payload.Nationality
                );

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var a = await mediator.Send(new GetNewTokenQuery(RequestDto.Create(LoginDtoV1.Create(user.Username, cmd.dto.Payload.Password), cmd.dto)), cancellationToken);
            // var b= await mediator.Send(new GetNewTokenQuery(cmd.dto.Request,  LoginDtoV1.Create(user.Username, cmd.Payload.Password)), cancellationToken);
            return a;
            //return new ResponseDto<TokenDtoV1>(a.Payload);
        }
    }
}
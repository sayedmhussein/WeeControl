#nullable enable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.DtoParent;
using WeeControl.Core.SharedKernel.ExtensionHelpers;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class UserRegisterCommand : IRequest<ResponseDto<TokenResponseDto>>
{
    private readonly PersonModel person;
    private readonly RequestDto request;

    public UserRegisterCommand(RequestDto<UserProfileDto> dto)
    {
        request = dto;

        dto.Payload.ThrowExceptionIfEntityModelNotValid();
        person = dto.Payload;
    }

    public class RegisterHandler : IRequestHandler<UserRegisterCommand, ResponseDto<TokenResponseDto>>
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

        public async Task<ResponseDto<TokenResponseDto>> Handle(UserRegisterCommand cmd,
            CancellationToken cancellationToken)
        {
            if (context.Person.Any(x =>
                    x.Username == cmd.person.Username.ToLower() ||
                    x.Email == cmd.person.Email.ToLower()
                ))
                throw new ConflictFailureException();

            var person = PersonDbo.Create(cmd.person);
            person.Password = passwordSecurity.Hash(cmd.person.Password);
            await context.Person.AddAsync(person, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var request =
                new SessionCreateCommand(RequestDto.Create(
                    LoginRequestDto.Create(person.Username, cmd.person.Password),
                    cmd.request));
            var response = await mediator.Send(request, cancellationToken);
            return response;
        }
    }
}
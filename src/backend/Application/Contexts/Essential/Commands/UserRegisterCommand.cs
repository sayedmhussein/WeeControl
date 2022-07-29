﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.Contexts.Essential.Commands;

public class UserRegisterCommand : IRequest<IResponseDto<TokenDtoV1>>
{
    private readonly IRequestDto<UserRegisterDto> dto;

    public UserRegisterCommand(IRequestDto<UserRegisterDto> dto)
    {
        this.dto = dto;
    }
    
    public class RegisterHandler : IRequestHandler<UserRegisterCommand, IResponseDto<TokenDtoV1>>
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

        public async Task<IResponseDto<TokenDtoV1>> Handle(UserRegisterCommand cmd, CancellationToken cancellationToken)
        {
            //await mediator.Send(new VerifyRequestQuery(cmd.dto), cancellationToken);

            if (string.IsNullOrWhiteSpace(cmd.dto.Payload.FirstName) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.LastName) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.Email) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.Username) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.Password) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.MobileNo) ||
                string.IsNullOrWhiteSpace(cmd.dto.Payload.TerritoryId)
               )
            {
                throw new ValidationException(cmd.dto.Payload);
            }

            if (context.Users.Any(x => 
                    x.Username == cmd.dto.Payload.Username.ToLower() ||
                    x.Email == cmd.dto.Payload.Email.ToLower() ||
                    x.MobileNo == cmd.dto.Payload.MobileNo.ToLower()
                    ))
            {
                throw new ConflictFailureException();
            }

            cmd.dto.Payload.Email = cmd.dto.Payload.Email.ToLower();
            cmd.dto.Payload.Username = cmd.dto.Payload.Username.ToLower();
            cmd.dto.Payload.MobileNo = cmd.dto.Payload.MobileNo.ToLower();

            var user = UserDbo.Create(cmd.dto.Payload);
            user.Password = passwordSecurity.Hash(cmd.dto.Payload.Password);

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var request =
                new UserTokenQuery(RequestDto.Create(LoginDtoV1.Create(user.Username, cmd.dto.Payload.Password),
                    cmd.dto));
            var response = await mediator.Send(request, cancellationToken);
            return response;
        }
    }
}
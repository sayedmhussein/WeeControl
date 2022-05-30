using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.EssentialContext.Notifications;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.DataTransferObjects.User;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Application.EssentialContext.Commands;

public class ForgotMyPasswordCommand : IRequest
{
    public IRequestDto Dto { get; }
    public string Email { get; }
    public string Username { get; }
    
    public ForgotMyPasswordCommand(RequestDto<ForgotMyPasswordDtoV1> dto)
    {
        Dto = dto;
        Email = dto.Payload.Email.ToLower();
        Username = dto.Payload.Username.ToLower();
    }
    
    public class ForgotMyPasswordHandler : IRequestHandler<ForgotMyPasswordCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly IMediator mediator;
        private readonly IPasswordSecurity passwordSecurity;

        public ForgotMyPasswordHandler(IEssentialDbContext context, IMediator mediator, IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.mediator = mediator;
            this.passwordSecurity = passwordSecurity;
        }
    
        public async Task<Unit> Handle(ForgotMyPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Dto.DeviceId) ||
                string.IsNullOrWhiteSpace(request.Email)||
                string.IsNullOrWhiteSpace(request.Username))
            {
                throw new BadRequestException("Invalid device or email or username");
            }
        
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.Email == request.Email, cancellationToken);
            if (user is not null)
            {
                if (string.IsNullOrEmpty(user.SuspendArgs) == false)
                    throw new NotAllowedException("Account is locked.");
                
                var password = passwordSecurity.GenerateRandomPassword();
                Console.WriteLine("New Password is: {0}", password);
                user.SetTemporaryPassword(passwordSecurity.Hash(password));
                await context.SaveChangesAsync(cancellationToken);
            
                await mediator.Publish(new PasswordReset(user.UserId, password), cancellationToken);
            
                return Unit.Value;
            }
        
        
            return Unit.Value;
        }
    }
}
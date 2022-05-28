using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.EssentialContext.Notifications;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.EssentialContext.Commands;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IEssentialDbContext context;
    private readonly IMediator mediator;
    private readonly IPasswordSecurity passwordSecurity;

    public ResetPasswordHandler(IEssentialDbContext context, IMediator mediator, IPasswordSecurity passwordSecurity)
    {
        this.context = context;
        this.mediator = mediator;
        this.passwordSecurity = passwordSecurity;
    }
    
    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Dto.DeviceId))
        {
            throw new BadRequestException("Invalid device");
        }
        
        var user = await context.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.Email == request.Email, cancellationToken);
        if (user is not null)
        {
            var password = passwordSecurity.GenerateRandomPassword();
            Console.WriteLine("New Password is: {0}", password);
            user.UpdatePassword(passwordSecurity.Hash(password));
            await context.SaveChangesAsync(cancellationToken);
            
            await mediator.Publish(new PasswordReset(user.UserId, password), cancellationToken);
            
            return Unit.Value;
        }
        
        
        return Unit.Value;
    }
}
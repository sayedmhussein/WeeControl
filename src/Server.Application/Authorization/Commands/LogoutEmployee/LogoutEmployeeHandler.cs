using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Common.Exceptions;
using WeeControl.Server.Domain.Authorization;
using WeeControl.SharedKernel.Common.DtosV1;

namespace WeeControl.Application.Authorization.Commands.LogoutEmployee
{
    public class LogoutEmployeeHandler : IRequestHandler<LogoutEmployeeCommand, ResponseDto>
    {
        private readonly IAuthorizationDbContext context;

        public LogoutEmployeeHandler(IAuthorizationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<ResponseDto> Handle(LogoutEmployeeCommand request, CancellationToken cancellationToken)
        {
            var session =
                await context.Sessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId && x.DeviceId == request.Device, cancellationToken);

            if (session is not null && session.TerminationTs is null)
            {
                session.TerminationTs = DateTime.UtcNow;
                await context.SaveChangesAsync(cancellationToken);
                return new ResponseDto(HttpStatusCode.OK);
            }
            
            throw new NotAllowedException("");
        }
    }
}
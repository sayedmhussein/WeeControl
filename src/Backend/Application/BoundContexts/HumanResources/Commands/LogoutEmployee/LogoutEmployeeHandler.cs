using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;

namespace WeeControl.Backend.Application.BoundContexts.HumanResources.Commands.LogoutEmployee
{
    public class LogoutEmployeeHandler : IRequestHandler<LogoutEmployeeCommand, ResponseDto>
    {
        private readonly IHumanResourcesDbContext context;

        public LogoutEmployeeHandler(IHumanResourcesDbContext context)
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
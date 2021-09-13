using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Backend.Application.Common.Behaviours
{
    public class RequestDtoBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo currentUser;

        public RequestDtoBehaviour(IMySystemDbContext context, ICurrentUserInfo currentUser)
        {
            this.context = context;
            this.currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IRequestDto dto)
            {
                await VerifyDeviceId(dto.DeviceId);
                await VerifyWithinTerritoryCredentials();
                
            }

            return await next();
        }

        private async Task VerifyDeviceId(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId))
            {
                throw new BadRequestException("Device ID was not provided!");
            }

            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == currentUser.SessionId);
            if (session.DeviceId != deviceId)
            {
                throw new BadRequestException("Device ID was not authorized!");
            }
        }

        private async Task VerifyWithinTerritoryCredentials()
        {
            await Task.CompletedTask;
        }
    }
}

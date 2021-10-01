using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Common.Interfaces;
using WeeControl.Server.Domain.HumanResources;
using WeeControl.SharedKernel.Common.Interfaces;

namespace WeeControl.Application.Common.Behaviours
{
    public class RequestDtoBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IHumanResourcesDbContext context;
        private readonly ICurrentUserInfo currentUser;

        public RequestDtoBehaviour(IHumanResourcesDbContext context, ICurrentUserInfo currentUser)
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

        private Task VerifyDeviceId(string deviceId)
        {
            // if (string.IsNullOrEmpty(deviceId))
            // {
            //     throw new BadRequestException("Device ID was not provided!");
            // }
            //
            // var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == currentUser.SessionId);
            // if (session.DeviceId != deviceId)
            // {
            //     throw new BadRequestException("Device ID was not authorized!");
            // }
            return Task.CompletedTask;
        }

        private async Task VerifyWithinTerritoryCredentials()
        {
            await Task.CompletedTask;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Backend.Application.Exceptions;

namespace WeeControl.Backend.Application.BoundContexts.Shared.Queries
{
    public class VerifyRequestHandler : IRequestHandler<VerifyRequestQuery>
    {
        public VerifyRequestHandler()
        {
        }

        public Task<Unit> Handle(VerifyRequestQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RequestDto.DeviceId))
            {
                throw new BadRequestException("Invalid Device");
            }

            return Unit.Task;
        }
    }
}

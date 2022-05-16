using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Exceptions;

namespace WeeControl.Application.EssentialContext.Queries;

public class VerifyRequestHandler : IRequestHandler<VerifyRequestQuery>
{
    public Task<Unit> Handle(VerifyRequestQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RequestDto.DeviceId))
        {
            throw new BadRequestException("Invalid Device");
        }

        return Unit.Task;
    }
}
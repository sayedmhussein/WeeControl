using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.Contexts.System.Queries;

public class VerifyRequestQuery : IRequest
{
    private IRequestDto RequestDto { get; }
    
    public VerifyRequestQuery(IRequestDto requestDto)
    {
        RequestDto = requestDto;
    }
    
    public class VerifyRequestHandler : IRequestHandler<VerifyRequestQuery>
    {
        public Task<Unit> Handle(VerifyRequestQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RequestDto.DeviceId))
            {
                throw new BadRequestException("Invalid Device");
            }

            if ((request.RequestDto.Latitude is null && request.RequestDto.Longitude is not null) ||
                (request.RequestDto.Latitude is not null && request.RequestDto.Longitude is null))
            {
                throw new BadRequestException("Either both Geo should be null or both not null!");
            }

            if (request.RequestDto.Latitude is > 90 or < -90)
            {
                throw new BadRequestException("Latitude is out of range!");
            }
            
            if (request.RequestDto.Longitude is > 180 or < -180)
            {
                throw new BadRequestException("Longitude is out of range!");
            }

            return Unit.Task;
        }
    }
}
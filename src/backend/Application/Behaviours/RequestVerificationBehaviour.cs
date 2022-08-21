using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace WeeControl.ApiApp.Application.Behaviours;

public class RequestVerificationBehaviour<TRequest> : IRequestPreProcessor<TRequest>
{
    
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        // if (string.IsNullOrWhiteSpace(request.RequestDto.DeviceId))
        // {
        //     throw new BadRequestException("Invalid Device");
        // }
        //
        // if ((request.RequestDto.Latitude is null && request.RequestDto.Longitude is not null) ||
        //     (request.RequestDto.Latitude is not null && request.RequestDto.Longitude is null))
        // {
        //     throw new BadRequestException("Either both Geo should be null or both not null!");
        // }
        //
        // if (request.RequestDto.Latitude is > 90 or < -90)
        // {
        //     throw new BadRequestException("Latitude is out of range!");
        // }
        //     
        // if (request.RequestDto.Longitude is > 180 or < -180)
        // {
        //     throw new BadRequestException("Longitude is out of range!");
        // }
        return Task.CompletedTask;
    }
}
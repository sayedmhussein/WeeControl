using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.Essential.Queries;

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

            return Unit.Task;
        }
    }
}
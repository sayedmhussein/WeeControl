using MediatR;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.EssentialContext.Queries;

public class VerifyRequestQuery : IRequest
{
    public VerifyRequestQuery(IRequestDto requestDto)
    {
        RequestDto = requestDto;
    }

    public IRequestDto RequestDto { get; }
}
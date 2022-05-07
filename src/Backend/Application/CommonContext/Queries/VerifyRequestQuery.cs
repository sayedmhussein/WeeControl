using MediatR;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Backend.Application.CommonContext.Queries
{
    public class VerifyRequestQuery : IRequest
    {
        public VerifyRequestQuery(IRequestDto requestDto)
        {
            RequestDto = requestDto;
        }

        public IRequestDto RequestDto { get; }
    }
}

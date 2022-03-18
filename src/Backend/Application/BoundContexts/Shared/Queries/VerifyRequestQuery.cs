using System;
using MediatR;
using WeeControl.Common.BoundedContext.Interfaces;

namespace WeeControl.Backend.Application.BoundContexts.Shared.Queries
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

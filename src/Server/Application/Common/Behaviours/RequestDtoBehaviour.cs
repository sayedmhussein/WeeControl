using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Server.Application.Common.Behaviours
{
    public class RequestDtoBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public RequestDtoBehaviour()
        {
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IAbstractRequestDto)
            {
                throw new NotImplementedException();
            }
            else
            {
                return await next();
            }
        }
    }
}

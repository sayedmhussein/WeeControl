using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Application.Behaviours;

public class RequestVerificationBehaviour<TRequest> : IRequestPreProcessor<TRequest>
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (request is IRequestDto dto)
        {
            dto.ThrowExceptionIfEntityModelNotValid();
        }

        return Task.CompletedTask;
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using WeeControl.Core.SharedKernel.DtoParent;
using WeeControl.Core.SharedKernel.ExtensionHelpers;

namespace WeeControl.Core.Application.Behaviours;

public class RequestVerificationBehaviour<TRequest> : IRequestPreProcessor<TRequest>
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (request is IRequestDto dto) dto.ThrowExceptionIfEntityModelNotValid();

        return Task.CompletedTask;
    }
}
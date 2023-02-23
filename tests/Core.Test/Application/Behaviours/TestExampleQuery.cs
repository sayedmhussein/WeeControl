using MediatR;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;

namespace WeeControl.Core.Test.Application.Behaviours;

public class TestExampleQuery : IRequest
{
    private int delay;
    private RequestDto<LoginRequestDto>? dto;

    public TestExampleQuery(int delay, RequestDto<LoginRequestDto>? dto = null)
    {
        this.delay = delay;
        this.dto = dto;
    }

    public class TestExampleHandler : IRequestHandler<TestExampleQuery, Unit>
    {
        public async Task<Unit> Handle(TestExampleQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(request.delay, cancellationToken);
            return Unit.Value;
        }
    }
}
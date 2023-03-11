using MediatR;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;

namespace WeeControl.Core.Test.Application.Behaviours;

public class TestExampleQuery : RequestDto, IRequest
{
    private int delay;

    public TestExampleQuery(int delay, RequestDto<LoginRequestDto>? dto = null) : base(dto)
    {
        this.delay = delay;
    }

    // public class TestExampleHandler : IRequestHandler<TestExampleQuery, Unit>
    // {
    //     public async Task<Unit> Handle(TestExampleQuery request, CancellationToken cancellationToken)
    //     {
    //         await Task.Delay(request.delay, cancellationToken);
    //         return Unit.Value;
    //     }
    // }
}
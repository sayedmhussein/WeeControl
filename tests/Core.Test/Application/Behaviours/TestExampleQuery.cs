using MediatR;

namespace WeeControl.Core.Test.Application.Behaviours;

public class TestExampleQuery : IRequest
{
    private int delay;

    public TestExampleQuery(int delay)
    {
        this.delay = delay;
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
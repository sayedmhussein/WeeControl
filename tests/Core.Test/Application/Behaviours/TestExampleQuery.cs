using MediatR;

namespace WeeControl.Core.Test.Application.Behaviours;

public class TestExampleQuery : IRequest<bool>
{
    private bool delay;

    public TestExampleQuery(bool delay)
    {
        this.delay = delay;
    }

    public class TestExampleHandler : IRequestHandler<TestExampleQuery, bool>
    {
        public async Task<bool> Handle(TestExampleQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken);
            return request.delay;
        }
    }
}
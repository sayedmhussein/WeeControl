using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WeeControl.Application.Test.Behaviours;

public class TestExampleQuery : IRequest<string>
{
    private int delay;
    
    public TestExampleQuery(int delay)
    {
        this.delay = delay;
    }
    
    public class TestExampleHandler : IRequestHandler<TestExampleQuery, string>
    {
        public async Task<string> Handle(TestExampleQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(request.delay, cancellationToken);
            return "Completed Task";
        }
    }
}
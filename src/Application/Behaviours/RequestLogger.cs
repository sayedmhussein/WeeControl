using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using WeeControl.Application.Essential.Notifications;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.Behaviours;

public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly ICurrentUserInfo currentUserService;
    private readonly IMediator mediator;


    public RequestLogger(ICurrentUserInfo currentUserService, IMediator mediator)
    {
        this.currentUserService = currentUserService;
        this.mediator = mediator;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).Name;

        await mediator.Publish(new UserActivityNotification(name, ""), cancellationToken);


    }
}
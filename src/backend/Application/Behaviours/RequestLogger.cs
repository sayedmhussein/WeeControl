﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using WeeControl.Core.Application.Contexts.Essentials.Notifications;

namespace WeeControl.Core.Application.Behaviours;

public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly IMediator mediator;

    public RequestLogger(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).Name;

        await mediator.Publish(new UserSessionLog("Application Mediator Log", name), cancellationToken);
    }
}
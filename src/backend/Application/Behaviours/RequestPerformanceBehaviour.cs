using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using WeeControl.ApiApp.Application.Interfaces;

namespace WeeControl.ApiApp.Application.Behaviours;

public class RequestPerformanceBehaviour<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private const int ThresholdTime = 500;
    private readonly Stopwatch timer;
    private readonly ILogger<RequestPerformanceBehaviour<TRequest, TResponse>> logger;
    private readonly ICurrentUserInfo currentUserService;

    public RequestPerformanceBehaviour(
        ILogger<RequestPerformanceBehaviour<TRequest, TResponse>> logger, 
        ICurrentUserInfo currentUserService)
    {
        timer = new Stopwatch();

        this.logger = logger;
        this.currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        CancellationToken cancellationToken, 
        RequestHandlerDelegate<TResponse> next)
    {
        timer.Start();
        var response = await next();
        timer.Stop();

        if (timer.ElapsedMilliseconds <= ThresholdTime) return response;
        
        var name = typeof(TRequest).Name;
        logger.LogWarning(
            "WeeControl Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}",
            name, 
            timer.ElapsedMilliseconds, 
            currentUserService.SessionId, 
            request);
        return response;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
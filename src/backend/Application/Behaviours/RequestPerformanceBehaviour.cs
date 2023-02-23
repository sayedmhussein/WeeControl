using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Core.Application.Behaviours;

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

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        timer.Start();
        var response = await next();
        timer.Stop();
    
        if (timer.ElapsedMilliseconds <= ThresholdTime) return response;
    
        var name = typeof(TRequest).Name;
        
        
        logger.Log(LogLevel.Warning, "WeeControl Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@Request}", 
            name, timer.ElapsedMilliseconds, currentUserService.SessionId, request);
        
        return response;
    }
}
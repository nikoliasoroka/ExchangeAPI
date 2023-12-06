using MediatR;
using System.Diagnostics;
using ExchangeAPI.Modules.Identity.Shared.Interfaces;

namespace ExchangeAPI.Modules.Identity.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ICurrentUserService _currentUserService;

    public PerformanceBehaviour(ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
        }

        return response;
    }
}
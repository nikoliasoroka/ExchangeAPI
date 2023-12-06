using ExchangeAPI.Shared.Common.Constants;
using ExchangeAPI.Shared.Common.Models.Result.Implementations;
using ExchangeAPI.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeAPI.Shared.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly bool _isProduction;

    public ApiExceptionFilterAttribute()
    {
        _isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is "Production";

        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();

        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleValidationException(context);
            return;
        }

        HandleUnhandledException(context);
    }

    private void HandleUnhandledException(ExceptionContext context)
    {
        Result result;

        if (_isProduction)
            result = Result.CreateFailed(ErrorModel.InternalServerError)
                .AddError("https://tools.ietf.org/html/rfc7231#section-6.6.1");
        else
            result = Result.CreateFailed(ErrorModel.InternalServerError, context.Exception.StackTrace)
                .AddError(context.Exception.Message);

        context.Result = new ObjectResult(result.ErrorInfo)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        Result result;

        if (_isProduction)
            result = Result.CreateFailed(exception.Message)
                .AddError("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        else
            result = Result.CreateFailed(exception.Message, exception.StackTrace)
                .AddErrors(exception.Errors.Values.SelectMany(error => error));

        context.Result = new BadRequestObjectResult(result.ErrorInfo);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        Result result;

        if (_isProduction)
            result = Result.CreateFailed(ErrorModel.ResourceNotFound)
                .AddError("https://tools.ietf.org/html/rfc7231#section-6.5.4");
        else
            result = Result.CreateFailed(ErrorModel.ResourceNotFound, context.Exception.StackTrace)
                .AddError(context.Exception.Message);

        context.Result = new NotFoundObjectResult(result.ErrorInfo);

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        Result result;

        if (_isProduction)
            result = Result.CreateFailed(ErrorModel.Unauthorized)
                .AddError("https://tools.ietf.org/html/rfc7235#section-3.1");
        else
            result = Result.CreateFailed(ErrorModel.Unauthorized, context.Exception.StackTrace)
                .AddError(context.Exception.Message);

        context.Result = new UnauthorizedObjectResult(result.ErrorInfo);

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        Result result;

        if (_isProduction)
            result = Result.CreateFailed(ErrorModel.Forbidden)
                .AddError("https://tools.ietf.org/html/rfc7231#section-6.5.3");
        else
            result = Result.CreateFailed(ErrorModel.Forbidden, context.Exception.StackTrace)
                .AddError(context.Exception.Message);

        context.Result = new ObjectResult(result.ErrorInfo)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;
    }
}
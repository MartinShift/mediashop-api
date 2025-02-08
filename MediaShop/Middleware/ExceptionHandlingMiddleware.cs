using MediaShop.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MediaShop.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            Exception currentEx = ex;
            while (currentEx != null)
            {
                logger.LogError("An exception occurred: {ExceptionType} {ExceptionMessage} {StackTrace}", currentEx.GetType().Name, currentEx.Message, currentEx.StackTrace);
                currentEx = currentEx.InnerException;
            }

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        if (exception is ArgumentException or  MediaShop.Exceptions.ValidationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else if (exception is NotFoundException or InvalidOperationException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            Details = (string?)null,
        };

        if (env.IsDevelopment())
        {
            response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Details = exception.StackTrace,
            };
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

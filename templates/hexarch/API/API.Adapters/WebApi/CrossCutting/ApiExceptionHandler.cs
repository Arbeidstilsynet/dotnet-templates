using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api;

internal static class ApiExceptionHandler
{
    public static Task Handle(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        context.Response.StatusCode = exception.GetStatusCode();
        return context.Response.WriteAsJsonAsync(exception.GetProblemDetails());
    }

    private static int GetStatusCode(this Exception? exception)
    {
        return exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            System.FormatException => StatusCodes.Status400BadRequest,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            SakNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    private static ProblemDetails GetProblemDetails(this Exception? exception)
    {
        return new()
        {
            Title = $"{exception?.GetType().Name ?? "Exception"} occured",
            Detail = exception?.Message ?? "An unexpected error occured",
            Status = exception.GetStatusCode(),
        };
    }
}

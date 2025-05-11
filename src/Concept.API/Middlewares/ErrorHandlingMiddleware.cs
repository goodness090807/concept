using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace Concept.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

                _logger.Error(exception, "發生未處理的錯誤: {Method} {Path} | TraceId: {TraceId}",
                    context.Request.Method, context.Request.Path, traceId);

                var problemDetails = new ProblemDetails
                {
                    Title = "服務發生錯誤",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = exception.Message,
                    Instance = $"{context.Request.Method} {context.Request.Path}",
                    Extensions =
                    {
                        ["traceId"] = traceId
                    }
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}

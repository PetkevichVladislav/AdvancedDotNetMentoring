using System.Collections.Immutable;
using System.Net;
using System.Text.Json;

namespace CartingService.API.Infrastructure.Middleware.ExceptionMiddleware
{
    internal class ExceptionMiddleware
    {
        private static readonly ImmutableDictionary<Type, ExceptionDetails> exceptionDetailsByExceptionType = new Dictionary<Type, ExceptionDetails>
        {
            { typeof(ArgumentException), new ExceptionDetails { Message = "Internal Error", StatusCode = StatusCodes.Status500InternalServerError } },
        }.ToImmutableDictionary();

        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var exceptionGuid = Guid.NewGuid();
            logger.LogError(exception, "Exception was thrown. Excetion id: {@guid}", exceptionGuid);
            var exceptionDetails = GetExceptionDetails(exception, exceptionGuid);
            await context.Response.WriteAsync(exceptionDetails);
        }

        private static string GetExceptionDetails(Exception exception, Guid exceptionGuid)
        {
            var isExceptionDetailsNotFound = !exceptionDetailsByExceptionType.TryGetValue(exception.GetType(), out var details);
            if (isExceptionDetailsNotFound)
            {
                details = new ExceptionDetails { Message = "Internal Error", StatusCode = StatusCodes.Status500InternalServerError };
            }

            details = details! with { exceptionId = exceptionGuid };
            return JsonSerializer.Serialize(details);
        }
    }
}

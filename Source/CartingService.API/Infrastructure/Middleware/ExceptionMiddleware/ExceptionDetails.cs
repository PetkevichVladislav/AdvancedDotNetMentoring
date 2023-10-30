namespace CartingService.API.Infrastructure.Middleware.ExceptionMiddleware
{
    internal record ExceptionDetails
    {
        public Guid exceptionId { get; init; } 

        public int StatusCode { get; init; }

        public string? Message { get; init; }

    }
}

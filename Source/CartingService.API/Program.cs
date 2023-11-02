using CartingService.API.Infrastructure.HATEOAS;
using CartingService.API.Infrastructure.Middleware.ExceptionMiddleware;
using CartingService.API.Infrastructure.Swagger;
using CartingService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCartingServices(builder.Configuration);
builder.Services.AddCartingSwagger();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Insert(0, new JsonHateoasFormatter());
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());
ILogger logger = loggerFactory.CreateLogger<Program>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCartingSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

 
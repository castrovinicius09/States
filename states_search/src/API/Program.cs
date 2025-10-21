using API.Extensions;
using API.Middleware;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddSettings(builder.Configuration)
    .AddSwaggerDependencies()
    .AddApplicationDependencies()
    .AddInfraDependencies()
    .AddRabbitMQ();

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapPost("/BuscarEstados", async ([FromServices] IStatesService statesService, CancellationToken cancellationToken) =>
{
    Result result = await statesService.GetStatesAsync(cancellationToken);

    return result.IsSuccess
        ? Results.Ok("Lista de Estados enviada")
        : Results.Problem(result.ErrorMessage);
});

await app.RunAsync();

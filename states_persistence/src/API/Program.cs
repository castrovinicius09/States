using API.Extensions;
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
    .AddApplicationDependencies()
    .AddInfraDependencies()
    .AddRabbitMQ()
    .AddSwaggerDependencies()
    .AddMinIODependencies(builder.Configuration);

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/BuscarListaArquivos", async ([FromServices] IStatesService statesService, CancellationToken cancellationToken) =>
{
    Result result = await statesService.GetStatesFileNamesAsync(cancellationToken);

    return result.IsSuccess
        ? Results.Ok(result.Data)
        : Results.Problem(result.ErrorMessage);
});

app.MapGet("/BaixarArquivos", async ([FromServices] IStatesService statesService, string nomeArquivo, CancellationToken cancellationToken) =>
{
    Result result = await statesService.GetFileByNameAsync(nomeArquivo, cancellationToken);

    return result.IsSuccess
        ? Results.File(fileStream: (Stream)result.Data!, "application/octet-stream", nomeArquivo)
        : Results.Problem(result.ErrorMessage);
});

await app.RunAsync();

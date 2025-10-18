using API.Extensions;
using API.SettingsModels;
using Application.Abstractions.Results;
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<StateHttpSettings>(
    builder.Configuration.GetSection(nameof(StateHttpSettings)));

builder.Services
    .AddSwaggerDependencies()
    .AddApplicationDependencies()
    .AddInfraDependencies();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/BuscarEstados", async ([FromServices] IStatesService statesService, CancellationToken cancellationToken) =>
{
    Result result = await statesService.GetStatesAsync(cancellationToken);

    return result.IsSuccess
        ? Results.Ok("States fetched successfully.")
        : Results.Problem(result.ErrorMessage);
});

await app.RunAsync();

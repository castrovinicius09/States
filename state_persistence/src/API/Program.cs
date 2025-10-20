using API.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

app.MapPost("/ChecarFila", (CancellationToken cancellationToken) =>
{
    return Results.Ok();
});

await app.RunAsync();

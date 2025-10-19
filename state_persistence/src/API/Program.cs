using API.Extensions;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddSettings(builder.Configuration)
    .AddApplicationDependencies()
    .AddInfraDependencies()
    .AddRabbitMQ();

builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();

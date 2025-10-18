using API.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSwaggerDependencies()
    .AddInfraDependencies();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();

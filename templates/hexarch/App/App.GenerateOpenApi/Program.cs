// See https://aka.ms/new-console-template for more information

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.ConfigureOpenApiSpec();
});

var app = builder.Build();
app.MapControllers();
app.MapOpenApi();

await app.RunAsync();

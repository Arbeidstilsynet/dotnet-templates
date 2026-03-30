// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appAssembly =
    typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.IAssemblyInfo).Assembly;

builder.Services.AddControllers().AddApplicationPart(appAssembly);
builder.Services.AddOpenApi(options =>
{
    options.ConfigureOpenApiSpec();
});

var app = builder.Build();

await app.RunAsync();

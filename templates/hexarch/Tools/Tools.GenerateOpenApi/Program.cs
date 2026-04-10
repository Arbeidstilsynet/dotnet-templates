// See https://aka.ms/new-console-template for more information

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appAssembly =
    typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.IAssemblyInfo).Assembly;

builder.Services
    .ConfigureMainApi()
    .AddApplicationPart(appAssembly);

var app = builder.Build();

await app.RunAsync();

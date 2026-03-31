// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

var appAssembly =
    typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.IAssemblyInfo).Assembly;

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddControllers().AddApplicationPart(appAssembly);
builder.Services.AddOpenApi(options =>
{
    options.ConfigureOpenApiSpec();
});

var app = builder.Build();

await app.RunAsync();

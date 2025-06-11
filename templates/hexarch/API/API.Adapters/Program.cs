using Arbeidstilsynet.Common.AspNetCore.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using IAssemblyInfo = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.IAssemblyInfo;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetRequired<AppSettings>();
var services = builder.Services;
var env = builder.Environment;

services.ConfigureStandardApi(IAssemblyInfo.AppName, appSettings.CorsConfiguration, env);

services.AddDomain();
services.AddInfrastructureServices(appSettings.DatabaseConfiguration);

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddStandardApi();

await app.RunAsync();

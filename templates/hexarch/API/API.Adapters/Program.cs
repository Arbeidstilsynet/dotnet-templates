using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using IAssemblyInfo = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.IAssemblyInfo;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetRequired<AppSettings>();
var services = builder.Services;
var env = builder.Environment;

services.ConfigureStandardApi(IAssemblyInfo.AppName, appSettings.ApiConfig, env);

services.AddDomain(appSettings.DomainConfig);
services.AddInfrastructure(appSettings.InfrastructureConfig);

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddStandardApi();

await app.RunAsync();

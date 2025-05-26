using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetRequired<AppSettings>();
var services = builder.Services;
var env = builder.Environment;

services.ConfigureApi();
services.ConfigureSwagger();
services.ConfigureLogging(env);
services.ConfigureOpenTelemetry(IAssemblyInfo.AppName);

services.AddDomain();
services.AddInfrastructureServices(appSettings.DatabaseConfiguration);

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddApi();

app.AddScalar();

await app.RunAsync();

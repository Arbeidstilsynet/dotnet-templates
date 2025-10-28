using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using IAssemblyInfo = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.IAssemblyInfo;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetRequired<AppSettings>();
var services = builder.Services;
var env = builder.Environment;

var appNameFromConfig = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME");
services.ConfigureStandardApi(
    string.IsNullOrEmpty(appNameFromConfig) ? IAssemblyInfo.AppName : appNameFromConfig,
    appSettings.ApiConfig,
    env
);

services.AddDomain(appSettings.DomainConfig);
services.AddInfrastructure(appSettings.InfrastructureConfig);

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddStandardApi();

// Apply migrations before running the application
using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
    await migrationService.RunMigrations();
}

await app.RunAsync();

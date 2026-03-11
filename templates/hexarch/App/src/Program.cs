using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.Common.FeatureFlags.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Infrastructure;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.DependencyInjection;
using IAssemblyInfo = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.IAssemblyInfo;

var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetRequired<AppSettings>();
var services = builder.Services;
var env = builder.Environment;

var appNameFromConfig = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME");
services.ConfigureStandardApi(
    string.IsNullOrEmpty(appNameFromConfig) ? IAssemblyInfo.AppName : appNameFromConfig,
    appSettings.ApiConfig,
    env,
    builder.Configuration,
    (provider) => [provider.GetRequiredService<IDatabaseMigrationService>().RunMigrations()]
);
services.AddFeatureFlags(appSettings.ApiConfig.FeatureFlagSettings);

services.AddDomain(appSettings.DomainConfig);
services.AddInfrastructure(appSettings.InfrastructureConfig);

var app = builder.Build();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.AddStandardApi(appSettings.ApiConfig);
app.MapFeatureFlagEndpoint();

await app.RunAsync();

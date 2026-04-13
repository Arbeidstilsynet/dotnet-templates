using System.Net;
using Arbeidstilsynet.Common.AspNetCore.Extensions.CrossCutting;
using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.Common.FeatureFlags.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

internal static class StartupExtensions
{
    public static IMvcBuilder ConfigureApi(this IServiceCollection services)
    {
        services.AddOpenApi(openApiOptions =>
            openApiOptions.ConfigureBasicOpenApiSpec(IAssemblyInfo.AppName)
        );

        return services.ConfigureStandardMvc();
    }

    public static IServiceCollection ConfigureApp(
        this IServiceCollection services,
        string appName,
        ApiConfiguration apiConfiguration,
        IWebHostEnvironment env,
        IConfiguration configurationRoot,
        StartupChecks? startupChecks = null
    )
    {
        services.AddLogging(configure =>
        {
            configure.AddConfiguration(configurationRoot);
        });

        services.ConfigureApi();
        services.AddStandardHealthChecks();

        if (startupChecks != null)
        {
            services.AddStartupChecks(startupChecks);
        }

        services.ConfigureOpenTelemetry(appName);

        services.ConfigureCors(apiConfiguration.Cors, env.IsDevelopment());
        if (apiConfiguration.AuthenticationConfiguration is { } authConfiguration)
        {
            services.AddStandardAuth(authConfiguration);
        }

        services.AddFeatureFlags(apiConfiguration.FeatureFlagSettings);

        return services;
    }

    public static WebApplication AddApi(this WebApplication app, ApiConfiguration apiConfiguration)
    {
        app.AddStandardApi(
            authConfiguration: apiConfiguration.AuthenticationConfiguration,
            options => options.AddExceptionMapping<SakNotFoundException>(HttpStatusCode.NotFound)
        );

        app.MapFeatureFlagEndpoint();

        return app;
    }
}

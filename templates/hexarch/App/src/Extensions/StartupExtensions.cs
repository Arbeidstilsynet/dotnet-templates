using System.Net;
using Arbeidstilsynet.Common.AspNetCore.Extensions.CrossCutting;
using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Extensions;

internal static class StartupExtensions
{
    public static IMvcBuilder ConfigureMainApi(this IServiceCollection services)
    {
        services.AddOpenApi(openApiOptions => openApiOptions.ConfigureBasicOpenApiSpec(IAssemblyInfo.AppName));

        return services.ConfigureStandardMvc();
    }
    
    public static IServiceCollection ConfigureStandardApi(
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

        services.ConfigureMainApi();
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

        return services;
    }

    public static WebApplication AddStandardApi(
        this WebApplication app,
        ApiConfiguration apiConfiguration
    )
    {
        app.AddStandardApi(
            authConfiguration: apiConfiguration.AuthenticationConfiguration,
            options => options.AddExceptionMapping<SakNotFoundException>(HttpStatusCode.NotFound)
        );

        return app;
    }
}

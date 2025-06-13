using System.Net;
using Arbeidstilsynet.Common.AspNetCore.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api.Extensions;

internal static class StartupExtensions
{
    public static IServiceCollection ConfigureStandardApi(
        this IServiceCollection services,
        string appName,
        ApiConfiguration apiConfiguration,
        IWebHostEnvironment env
    )
    {
        services.ConfigureApi();
        services.ConfigureOpenTelemetry(appName);
        services.ConfigureSwagger();
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.SetMinimumLevel(LogLevel.Information);
        });
        services.ConfigureCors(
            apiConfiguration.Cors.AllowedOrigins,
            apiConfiguration.Cors.AllowCredentials,
            env.IsDevelopment()
        );

        return services;
    }

    public static WebApplication AddStandardApi(this WebApplication app)
    {
        app.AddApi(options =>
            options.AddExceptionMapping<SakNotFoundException>(HttpStatusCode.NotFound)
        );
        app.UseCors();
        app.AddScalar();

        return app;
    }
}

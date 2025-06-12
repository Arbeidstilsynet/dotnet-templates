using System.Net;
using Arbeidstilsynet.Common.AspNetCore.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api.Extensions;

internal static class StartupExtensions
{
    public static IServiceCollection ConfigureStandardApi(
        this IServiceCollection services,
        string appName,
        CorsConfiguration corsConfiguration,
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
            corsConfiguration.AllowedOrigins,
            corsConfiguration.AllowCredentials,
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

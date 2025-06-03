using System.Net;
using Arbeidstilsynet.Common.AspNetCore.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

namespace API.Adapters.Extensions;

internal static class StartupExtensions
{
    public static IServiceCollection ConfigureStandardApi(
        this IServiceCollection services,
        string appName,
        IWebHostEnvironment env
    )
    {
        services.ConfigureApi();
        services.ConfigureOpenTelemetry(appName);
        services.ConfigureSwagger();
        services.ConfigureLogging(env);

        return services;
    }

    public static WebApplication AddStandardApi(this WebApplication app)
    {
        app.AddApi(options =>
            options.AddExceptionMapping<SakNotFoundException>(HttpStatusCode.NotFound)
        );
        app.AddScalar();

        return app;
    }
}

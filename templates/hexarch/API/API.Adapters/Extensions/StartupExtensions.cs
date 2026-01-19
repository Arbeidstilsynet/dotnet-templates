using System.Net;
using Arbeidstilsynet.Common.AspNetCore.Extensions.CrossCutting;
using Arbeidstilsynet.Common.AspNetCore.Extensions.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Extensions;

internal static class StartupExtensions
{
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
        services.ConfigureApi(
            startupChecks: startupChecks,
            buildHealthChecksAction: builder => builder.AddInfrastructureHealthChecks()
        );
        services.ConfigureOpenTelemetry(appName);
        services.ConfigureOpenApi();

        services.ConfigureCors(
            apiConfiguration.Cors.AllowedOrigins,
            apiConfiguration.Cors.AllowCredentials,
            env.IsDevelopment()
        );

        if (apiConfiguration.AuthenticationConfiguration.DangerousDisableAuth)
        {
            LoggerFactory
                .Create(builder => builder.AddConsole())
                .CreateLogger<Program>()
                .LogWarning(
                    "Authentication is disabled. Update AuthenticationConfiguration to require authentication."
                );

            // Register a permissive authorization policy that allows all requests
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true)
                    .Build();
            });
        }
        else
        {
            var clientId = apiConfiguration.AuthenticationConfiguration.EntraClientId;
            var tenantId = apiConfiguration.AuthenticationConfiguration.EntraTenantId;

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException(
                    "EntraClientId must be set either in appsettings when auth is enabled"
                );
            }
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentException("EntraTenantId must be set when auth is enabled");
            }

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
                    jwtOptions.Audience = clientId;
                });

            services.AddAuthorization();
        }
        return services;
    }

    public static WebApplication AddStandardApi(
        this WebApplication app,
        ApiConfiguration apiConfiguration
    )
    {
        if (!apiConfiguration.AuthenticationConfiguration.DangerousDisableAuth)
        {
            app.UseAuthentication();
        }
        app.UseAuthorization();

        app.AddApi(options =>
            options.AddExceptionMapping<SakNotFoundException>(HttpStatusCode.NotFound)
        );
        app.UseCors();
        app.AddScalar();

        return app;
    }
}

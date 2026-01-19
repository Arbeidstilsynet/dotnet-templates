using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.Common.FeatureFlags.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;

/// <summary>
/// This is the root configuration object for the application. It is bound from <see cref="IConfiguration"/>.
/// It should contain all configuration required for the application to run.
/// </summary>
/// <remarks>
/// When adding configuration, it should fit into one of these categories:
/// <br/>- <see cref="ApiConfiguration"/>
/// <br/>- <see cref="DomainConfiguration"/>
/// <br/>- <see cref="InfrastructureConfiguration"/>
/// </remarks>
internal record AppSettings
{
    /// <summary>
    /// Configures user-facing functionality.
    /// </summary>
    [Required]
    [ConfigurationKeyName("API")]
    public required ApiConfiguration ApiConfig { get; init; }

    /// <summary>
    /// Required to access and utilize infrastructure.
    /// </summary>
    [Required]
    [ConfigurationKeyName("Infrastructure")]
    public required InfrastructureConfiguration InfrastructureConfig { get; init; }

    /// <summary>
    /// Configures the internal domain logic.
    /// </summary>
    [Required]
    [ConfigurationKeyName("Domain")]
    public required DomainConfiguration DomainConfig { get; init; }
}

/// <summary>
/// Configures the user-facing interface of the application, e.g. CORS, Auth, rate limiting etc.
/// </summary>
internal record ApiConfiguration
{
    [ConfigurationKeyName("Cors")]
    public CorsConfiguration Cors { get; init; } = new();

    [ConfigurationKeyName("Authentication")]
    public AuthConfiguration AuthenticationConfiguration { get; init; } = new();

    [ConfigurationKeyName("FeatureFlag")]
    public FeatureFlagSettings FeatureFlagSettings { get; init; } = new();
}

internal record CorsConfiguration
{
    [Required]
    public string[] AllowedOrigins { get; init; } = [];

    [Required]
    public bool AllowCredentials { get; init; } = false;
}

internal record AuthConfiguration
{
    [ConfigurationKeyName("DangerousDisableAuth")]
    public bool DangerousDisableAuth { get; init; } = false;

    [ConfigurationKeyName("TenantId")]
    public string EntraTenantId { get; init; } = string.Empty;

    [ConfigurationKeyName("ClientId")]
    public string EntraClientId { get; init; } = string.Empty;
}

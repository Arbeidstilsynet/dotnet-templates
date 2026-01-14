using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;


/// <summary>
/// This is the root configuration object for the application. It is bound from <see cref="IConfiguration"/>.
/// </summary>
/// <remarks>
/// When adding configuration, it should fit into one of these categories:
/// <br/>- API
/// <br/>- Domain
/// <br/>- Infrastructure
/// </remarks>
internal record AppSettings
{
    [ConfigurationKeyName("API")]
    [Required]
    public required ApiConfiguration ApiConfig { get; init; }

    [Required]
    [ConfigurationKeyName("Infrastructure")]
    public required InfrastructureConfiguration InfrastructureConfig { get; init; }

    /// <summary>
    /// Configuration related to the Domain layer of the application.
    /// </summary>
    [Required]
    [ConfigurationKeyName("Domain")]
    public required DomainConfiguration DomainConfig { get; init; }
}


/// <summary>
/// Data that configures the interface of the application, e.g. CORS, Auth, RateLimiting etc.
/// </summary>
internal record ApiConfiguration
{
    [ConfigurationKeyName("Cors")]
    public CorsConfiguration Cors { get; init; } = new();
    
    [ConfigurationKeyName("Auth")]
    [Required]
    public required AuthConfiguration Auth { get; init; }
}

internal record AuthConfiguration
{
    [ConfigurationKeyName("DangerousDisableAuth")]
    public bool DangerousDisableAuth { get; init; } = false;
    
    [ConfigurationKeyName("Authority")]
    [Required]
    public required string Authority { get; init; }

    [ConfigurationKeyName("Audience")]
    [Required]
    public required string Audience { get; init; }
}

internal record CorsConfiguration
{
    [Required]
    public string[] AllowedOrigins { get; init; } = [];

    [Required]
    public bool AllowCredentials { get; init; } = false;
}

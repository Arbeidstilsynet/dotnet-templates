using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api;

internal record AppSettings
{
    [ConfigurationKeyName("API")]
    public ApiConfiguration ApiConfig { get; init; } = new();

    [Required]
    [ConfigurationKeyName("Infrastructure")]
    public required InfrastructureConfiguration InfrastructureConfig { get; init; }

    [ConfigurationKeyName("Domain")]
    public required DomainConfiguration DomainConfig { get; init; }
}

internal record ApiConfiguration
{
    [ConfigurationKeyName("Cors")]
    public CorsConfiguration Cors { get; init; } = new();
}

internal record CorsConfiguration
{
    [Required]
    public string[] AllowedOrigins { get; init; } = [];

    [Required]
    public bool AllowCredentials { get; init; } = false;
}

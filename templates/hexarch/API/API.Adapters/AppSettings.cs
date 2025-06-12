using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Api;

internal record AppSettings
{
    [Required]
    [ConfigurationKeyName("DatabaseConfiguration")]
    public required DatabaseConfiguration DatabaseConfiguration { get; init; }

    [ConfigurationKeyName("CorsConfiguration")]
    public CorsConfiguration CorsConfiguration { get; init; } = new();
}

internal record CorsConfiguration
{
    public string[] AllowedOrigins { get; init; } = [];
    public bool AllowCredentials { get; init; } = false;
}

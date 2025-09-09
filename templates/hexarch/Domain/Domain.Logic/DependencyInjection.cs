using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;

public class DomainConfiguration
{
    [Required]
    public required string SomeSetting { get; init; }
}

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        DomainConfiguration domainConfiguration
    )
    {
        services.AddSingleton(Options.Create(domainConfiguration));
        services.AddScoped<ISakService, SakService>();
        return services;
    }
}

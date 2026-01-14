using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;


/// <summary>
/// Domain specific configuration, e.g. business rules, limits etc.
/// </summary>
public class DomainConfiguration
{
    /// <summary>
    /// The length of time before a <see cref="Sak"/> deadline is reached.
    /// </summary>
    [Required]
    public required int SakDeadlineDays { get; init; }
}

/// <summary>
/// Extension methods to add domain services to the DI container.
/// </summary>
public static class DependencyInjection
{
    
    /// <summary>
    /// Adds domain functionality to the application. This usually includes domain logic, and sometimes configuration.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="domainConfiguration"></param>
    /// <returns></returns>
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

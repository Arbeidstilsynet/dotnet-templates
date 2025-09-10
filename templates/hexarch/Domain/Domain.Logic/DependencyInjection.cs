using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

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
        services.AddMapper();
        services.AddSingleton(Options.Create(domainConfiguration));
        services.AddScoped<ISakService, SakService>();
        return services;
    }

    internal static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var existingConfig = services
            .Select(s => s.ImplementationInstance)
            .OfType<TypeAdapterConfig>()
            .FirstOrDefault();

        if (existingConfig == null)
        {
            var config = new TypeAdapterConfig()
            {
                RequireExplicitMapping = false,
                RequireDestinationMemberSource = true,
            };
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }
        else
        {
            existingConfig.Scan(Assembly.GetExecutingAssembly());
        }
        return services;
    }
}

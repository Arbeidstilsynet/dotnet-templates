using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;

public record InfrastructureConfiguration
{
    [Required]
    public required string ConnectionString { get; init; }
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        InfrastructureConfiguration infrastructureConfiguration
    )
    {
        services.AddScoped<ISakRepository, SakRepository>();
        services.AddSingleton(infrastructureConfiguration);
        services.AddDbContext<SakDbContext>(opt =>
        {
            opt.UseNpgsql(infrastructureConfiguration.ConnectionString);
        });

        services.AddMapper();
        services.AddHealthChecks().AddDbContextCheck<SakDbContext>();

        return services;
    }

    private static IServiceCollection AddMapper(this IServiceCollection services)
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

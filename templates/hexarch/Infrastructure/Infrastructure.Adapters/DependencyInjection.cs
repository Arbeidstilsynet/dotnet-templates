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

public record DatabaseConfiguration
{
    [Required]
    public required string ConnectionString { get; set; }
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration
    )
    {
        services.AddScoped<ISakRepository, SakRepository>();
        services.AddSingleton(databaseConfiguration);
        services.AddDbContext<InfrastructureAdaptersDbContext>(opt =>
        {
            opt.UseNpgsql(databaseConfiguration.ConnectionString);
        });

        services.AddMapper();
        services.AddHealthChecks().AddCheck<DbContextHealthCheck>("DbContextCheck");

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
                RequireExplicitMapping = true,
                RequireDestinationMemberSource = false,
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

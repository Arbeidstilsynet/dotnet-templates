using System.ComponentModel.DataAnnotations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
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

        services.AddHealthChecks().AddDbContextCheck<SakDbContext>();

        return services;
    }
}

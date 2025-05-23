using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;

internal class DbContextHealthCheck : IHealthCheck
{
    private readonly InfrastructureAdaptersDbContext _dbContext;

    public DbContextHealthCheck(InfrastructureAdaptersDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        if (await _dbContext.Database.CanConnectAsync(cancellationToken))
        {
            return await Task.FromResult(
                HealthCheckResult.Healthy("Successfully validated db connection.")
            );
        }

        return await Task.FromResult(
            new HealthCheckResult(context.Registration.FailureStatus, "Could not reach database.")
        );
    }
}

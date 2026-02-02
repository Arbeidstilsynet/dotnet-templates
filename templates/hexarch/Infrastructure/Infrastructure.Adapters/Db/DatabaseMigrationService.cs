using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;

internal class DatabaseMigrationService(
    SakDbContext dbContext,
    ILogger<DatabaseMigrationService> logger
) : IDatabaseMigrationService
{
    public async Task RunMigrations()
    {
        logger.LogInformation("Applying database migrations...");
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully");
    }
}

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Infrastructure;

public interface IDatabaseMigrationService
{
    /// <summary>
    /// Executes all pending database migrations asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous migration operation.</returns>
    Task RunMigrations();
}

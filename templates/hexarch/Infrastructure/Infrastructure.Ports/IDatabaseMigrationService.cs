namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;

public interface IDatabaseMigrationService
{
    /// <summary>
    /// Executes all pending database migrations asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous migration operation.</returns>
    Task RunMigrations();
}

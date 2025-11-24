using Testcontainers.PostgreSql;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;

public class PostgresDbDemoFixture
{
    private readonly PostgreSqlContainer _sqlContainer = new PostgreSqlBuilder().Build();

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public Task InitializeAsync()
    {
        return _sqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _sqlContainer.StopAsync();
    }
}

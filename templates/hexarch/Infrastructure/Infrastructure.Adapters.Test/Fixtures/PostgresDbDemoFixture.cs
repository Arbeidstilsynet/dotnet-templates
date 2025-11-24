using Testcontainers.PostgreSql;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;

public class PostgresDbDemoFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _sqlContainer = new PostgreSqlBuilder().Build();

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public ValueTask InitializeAsync()
    {
        return new ValueTask(_sqlContainer.StartAsync());
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask(_sqlContainer.StopAsync());
    }
}

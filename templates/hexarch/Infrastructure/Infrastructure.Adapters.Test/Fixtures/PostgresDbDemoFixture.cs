using Testcontainers.PostgreSql;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;

public class PostgresDbDemoFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _sqlContainer = new PostgreSqlBuilder(
        "postgres:17-alpine@sha256:9a78577340f3d26384b6aebeb475c0d46d664fd4ffa68503b4be4e4462745f94"
    )
        .WithCleanUp(true)
        .Build();

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public ValueTask InitializeAsync()
    {
        return new ValueTask(_sqlContainer.StartAsync());
    }

    public ValueTask DisposeAsync()
    {
        return _sqlContainer.DisposeAsync();
    }
}

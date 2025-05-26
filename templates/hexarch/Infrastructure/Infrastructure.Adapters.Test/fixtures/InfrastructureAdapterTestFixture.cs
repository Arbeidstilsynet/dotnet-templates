using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;

public class InfrastructureAdapterTestFixture : TestBedFixture, IAsyncLifetime
{
    private readonly PostgresDbDemoFixture _dbDemoFixture;

    public InfrastructureAdapterTestFixture()
    {
        _dbDemoFixture = new PostgresDbDemoFixture();
    }

    protected override void AddServices(
        IServiceCollection services,
        global::Microsoft.Extensions.Configuration.IConfiguration? configuration
    )
    {
        services.AddInfrastructureServices(
            new DatabaseConfiguration() { ConnectionString = _dbDemoFixture.ConnectionString }
        );
    }

    protected override ValueTask DisposeAsyncCore() => new();

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = true };
    }

    public Task InitializeAsync()
    {
        return _dbDemoFixture.InitializeAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbDemoFixture.DisposeAsync();
    }
}

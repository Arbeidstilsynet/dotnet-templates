using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.DependencyInjection;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.v3;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Test.Fixtures;

public class InfrastructureAdapterTestFixture : TestBedFixture, IAsyncLifetime
{
    private readonly TestOutputHelper _testOutputHelper = new();
    private readonly PostgresDbDemoFixture _dbDemoFixture = new();

    private readonly Faker<SakEntity> _sakEntityFaker = TestData.CreateSakEntityFaker();
    internal IReadOnlyList<SakEntity> SeededEntities { get; }

    public InfrastructureAdapterTestFixture()
    {
        SeededEntities = _sakEntityFaker.Generate(50);
    }

    protected override void AddServices(
        IServiceCollection services,
        global::Microsoft.Extensions.Configuration.IConfiguration? configuration
    )
    {
        services.AddInfrastructure(
            new InfrastructureConfiguration() { ConnectionString = _dbDemoFixture.ConnectionString }
        );
    }

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = true };
    }

    protected override ValueTask DisposeAsyncCore()
    {
        return _dbDemoFixture.DisposeAsync();
    }

    private async Task SeedDatabase()
    {
        var dbContext = GetService<SakDbContext>(_testOutputHelper)!;

        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Saker.AddRangeAsync(SeededEntities);
        await dbContext.SaveChangesAsync();
    }

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await _dbDemoFixture.InitializeAsync();
        await SeedDatabase();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await DisposeAsyncCore();
    }
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.Fixture;

public class ApplicationFixture : WebApplicationFactory<IAssemblyInfo>, IAsyncLifetime
{
    private readonly PostgresDbDemoFixture _postgresDbDemoFixture = new();

    private Tilsynssak? _seededSak;
    internal Tilsynssak SeededTilsynssak =>
        _seededSak ?? throw new InvalidOperationException("Database not seeded yet");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // remove background jobs from startup
            services.RemoveAll<IHostedService>();
            services.RemoveAll<InfrastructureConfiguration>();
            services.AddSingleton(
                new InfrastructureConfiguration
                {
                    ConnectionString = _postgresDbDemoFixture.ConnectionString,
                }
            );
            services.RemoveAll<TilsynssakDbContext>();
            services.RemoveAll<DbContextOptions<TilsynssakDbContext>>();
            services.AddDbContext<TilsynssakDbContext>(opt =>
            {
                opt.UseNpgsql(_postgresDbDemoFixture.ConnectionString);
            });
        });
    }

    private async Task RunMigrations()
    {
        using var scope = Services.CreateScope();
        var migrationService =
            scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
        await migrationService.RunMigrations();
    }

    private async Task SeedDatabase()
    {
        using var scope = Services.CreateScope();
        var sakService = scope.ServiceProvider.GetRequiredService<ITilsynssakService>();

        _seededSak = await sakService.CreateNewSak(
            new CreateTilsynssakDto() { Organisajonsnummer = "123456789" }
        );
    }

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await _postgresDbDemoFixture.InitializeAsync();
        await RunMigrations();
        await SeedDatabase();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await _postgresDbDemoFixture.DisposeAsync();
    }
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Migrations;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class MigrationServiceTests : IAsyncLifetime
{
    PostgresDbDemoFixture _dbFixture = new();

    private readonly Guid _oldSakId = new("11111111-1111-1111-1111-111111111111");
    private readonly DateTime _createdTime;

    public MigrationServiceTests()
    {
        var dt = new DateTime(2023, 1, 1);
        _createdTime = dt.ToUniversalTime();
    }

    [Fact]
    public async Task RunMigrations_WhenCalled_AppliesMigrationsAsync()
    {
        await using var dbContext = new SakDbContext(
            new DbContextOptionsBuilder<SakDbContext>()
                .UseNpgsql(_dbFixture.ConnectionString)
                .Options
        );

        var sut = new DatabaseMigrationService(
            dbContext,
            Substitute.For<ILogger<DatabaseMigrationService>>()
        );
        await sut.RunMigrations();

        var hits = await dbContext
            .Saker.Where(s => s.Id == _oldSakId)
            .ToListAsync(TestContext.Current.CancellationToken);

        hits.ShouldHaveSingleItem();

        hits[0].Id.ShouldBe(_oldSakId);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbFixture.DisposeAsync();
    }

    public async ValueTask InitializeAsync()
    {
        await _dbFixture.InitializeAsync();
        // Run first migration to create initial schema

        await using (
            var dbContextBeforeMigration = new SakDbContext(
                new DbContextOptionsBuilder<SakDbContext>()
                    .UseNpgsql(_dbFixture.ConnectionString)
                    .Options
            )
        )
        {
            // Run the first migration only. Migrations are located under Infrastructure/Infrastructure.Adapters/Adapters/Db/Migrations
            await dbContextBeforeMigration.Database.MigrateAsync("20251124064046_InitDb");

            await dbContextBeforeMigration.SaveChangesAsync();
        }

        await using (
            var dbContext = new SakDbContext(
                new DbContextOptionsBuilder<SakDbContext>()
                    .UseNpgsql(_dbFixture.ConnectionString)
                    .Options
            )
        )
        {
            // Insert v1 schema data to simulate an old database state (using unsafe injection-prone method)
#pragma warning disable EF1002
            await dbContext.Database.ExecuteSqlRawAsync(
                $"""
                INSERT INTO "Saker" ("Id", "CreatedAt", "UpdatedAt", "Organisajonsnummer", "Status") 
                              VALUES ('{_oldSakId}', TIMESTAMP '{_createdTime:yyyy-MM-dd HH:mm:ss}', TIMESTAMP '{_createdTime:yyyy-MM-dd HH:mm:ss}', '123456789', 'New')
                """
#pragma warning restore EF1002
            );
        }
    }
}

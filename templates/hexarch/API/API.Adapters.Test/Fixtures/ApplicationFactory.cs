using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.fixture;

public class ApplicationFactory : WebApplicationFactory<IAssemblyInfo>, IAsyncLifetime
{
    private readonly PostgresDbDemoFixture _postgresDbDemoFixture = new();

    internal Sak SeededSak { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<InfrastructureConfiguration>();
            services.AddSingleton(
                new InfrastructureConfiguration
                {
                    ConnectionString = _postgresDbDemoFixture.ConnectionString,
                }
            );
            services.RemoveAll<SakDbContext>();
            services.AddDbContext<SakDbContext>(opt =>
            {
                opt.UseNpgsql(_postgresDbDemoFixture.ConnectionString);
            });
        });
    }

    private async Task SeedDatabase()
    {
        using var scope = Services.CreateScope();
        var sakService = scope.ServiceProvider.GetRequiredService<ISakService>();

        SeededSak = await sakService.CreateNewSak(
            new CreateSakDto() { Organisajonsnummer = "123456789" }
        );
    }

    public async Task InitializeAsync()
    {
        await _postgresDbDemoFixture.InitializeAsync();
        await SeedDatabase();
    }

    public new async Task DisposeAsync()
    {
        await _postgresDbDemoFixture.DisposeAsync();
    }
}

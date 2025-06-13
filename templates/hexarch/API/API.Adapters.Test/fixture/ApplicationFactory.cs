using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.fixture;

public class ApplicationFactory : WebApplicationFactory<API.Adapters.IAssemblyInfo>, IAsyncLifetime
{
    private readonly PostgresDbDemoFixture postgresDbDemoFixture = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<InfrastructureConfiguration>();
            services.AddSingleton(
                new InfrastructureConfiguration
                {
                    ConnectionString = postgresDbDemoFixture.ConnectionString,
                }
            );
            // replacing the current db context
            var context = services.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(InfrastructureAdaptersDbContext)
            );
            if (context != null)
            {
                services.Remove(context);
                var options = services
                    .Where(r =>
                        (r.ServiceType == typeof(DbContextOptions))
                        || (
                            r.ServiceType.IsGenericType
                            && r.ServiceType.GetGenericTypeDefinition()
                                == typeof(DbContextOptions<>)
                        )
                    )
                    .ToArray();
                foreach (var option in options)
                {
                    services.Remove(option);
                }
            }
            services.AddDbContext<InfrastructureAdaptersDbContext>(opt =>
            {
                opt.UseNpgsql(postgresDbDemoFixture.ConnectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        await postgresDbDemoFixture.InitializeAsync();
    }

    public new async Task DisposeAsync()
    {
        await postgresDbDemoFixture.DisposeAsync();
    }
}

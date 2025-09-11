using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.Test.Fixtures;

public class ApplicationTestFixture : TestBedFixture
{
    protected override void AddServices(
        IServiceCollection services,
        global::Microsoft.Extensions.Configuration.IConfiguration? configuration
    )
    {
        services.AddMapper();
    }

    protected override ValueTask DisposeAsyncCore() => new();

    protected override IEnumerable<TestAppSettings> GetTestAppSettings()
    {
        yield return new() { Filename = "appsettings.json", IsOptional = true };
    }
}

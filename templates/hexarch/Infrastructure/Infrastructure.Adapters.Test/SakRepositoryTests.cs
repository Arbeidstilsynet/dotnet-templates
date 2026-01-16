using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Shouldly;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class SakRepositoryTests : TestBed<InfrastructureAdapterTestFixture>
{
    private readonly ISakRepository _sut;

    private static readonly string SampleOrgNr = "123456789";

    private readonly VerifySettings _verifierSettings = new();

    public SakRepositoryTests(ITestOutputHelper testOutputHelper,
        InfrastructureAdapterTestFixture infrastractureAdapterTestFixture) : base(testOutputHelper, infrastractureAdapterTestFixture)
    {
        _sut = infrastractureAdapterTestFixture.GetService<ISakRepository>(testOutputHelper)!;
        
        _verifierSettings.DontScrubGuids();
        _verifierSettings.UseDirectory("Snapshots");
    }

    [Fact]
    public async Task CreateSak_WhenCalled_PersistsSakEntityAsync()
    {
        // arrange
        var newSak = TestData.CreateSakFaker(1).Generate()
            with
        {
            Organisajonsnummer = SampleOrgNr
        };
        
        // act
        var createdSak = await _sut.PersistSak(newSak);
        // assert
        var result = await _sut.GetSak(createdSak.Id);
        await Verify(result, _verifierSettings);
    }

    [Fact]
    public async Task UpdateSakStatus_WhenCalled_PersistsSakEntityAsync()
    {
        // arrange
        var createdSak = TestData.CreateSakFaker(2).Generate()
            with
        {
            Organisajonsnummer = SampleOrgNr
        };
        await _sut.PersistSak(createdSak);
        // act
        var updatedSak = await _sut.UpdateSakStatus(createdSak.Id, SakStatus.InProgress);
        // assert
        await Verify(updatedSak, _verifierSettings);
    }

    [Fact]
    public async Task GetSaker_WhenCalled_ReturnsAllSaker()
    {
        // arrange
        var seed = _fixture.SeededEntities;
        // act
        var allSaker = await _sut.GetSaker();
        // assert
        seed.Select(sak => sak.Id).ToList().ShouldBeSubsetOf([.. allSaker.Select(sak => sak.Id)]);
    }
}

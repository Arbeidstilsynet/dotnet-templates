using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Shouldly;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driven;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class SakRepositoryTests(
    ITestOutputHelper testOutputHelper,
    InfrastructureAdapterTestFixture infrastractureAdapterTestFixture
) : TestBed<InfrastructureAdapterTestFixture>(testOutputHelper, infrastractureAdapterTestFixture)
{
    private readonly ISaveSaker _sutSave = infrastractureAdapterTestFixture.GetService<ISaveSaker>(testOutputHelper)!;
    private readonly IGetSaker _sutGet = infrastractureAdapterTestFixture.GetService<IGetSaker>(testOutputHelper)!;

    private static readonly string SampleOrgNr = "123456789";

    [Fact]
    public async Task CreateSak_WhenCalled_PersistsSakEntityAsync()
    {
        // act
        var createdSak = await _sutSave.PersistSak(SampleOrgNr);
        // assert
        var result = await _sutGet.GetSak(createdSak.Id);
        result?.Organisajonsnummer.ShouldBe(SampleOrgNr);
    }

    [Fact]
    public async Task UpdateSakStatus_WhenCalled_PersistsSakEntityAsync()
    {
        // arrange
        var createdSak = await _sutSave.PersistSak(SampleOrgNr);
        // act
        var updatedSak = await _sutSave.UpdateSakStatus(createdSak.Id, SakStatus.InProgress);
        // assert
        updatedSak.ShouldBeEquivalentTo(
            createdSak with
            {
                Status = SakStatus.InProgress,
                LastUpdated = updatedSak!.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task GetSaker_WhenCalled_ReturnsAllSaker()
    {
        // arrange
        var seed = _fixture.SeededEntities;
        // act
        var allSaker = await _sutGet.GetSaker();
        // assert
        seed.Select(sak => sak.Id).ToList().ShouldBeSubsetOf([.. allSaker.Select(sak => sak.Id)]);
    }
}

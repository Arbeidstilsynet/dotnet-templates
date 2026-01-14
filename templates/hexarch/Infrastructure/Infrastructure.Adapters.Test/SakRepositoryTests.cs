using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Bogus;
using Shouldly;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class SakRepositoryTests(
    ITestOutputHelper testOutputHelper,
    InfrastructureAdapterTestFixture infrastractureAdapterTestFixture
) : TestBed<InfrastructureAdapterTestFixture>(testOutputHelper, infrastractureAdapterTestFixture)
{
    private readonly ISakRepository _sut =
        infrastractureAdapterTestFixture.GetService<ISakRepository>(testOutputHelper)!;

    private static readonly string SampleOrgNr = "123456789";

    [Fact]
    public async Task CreateSak_WhenCalled_PersistsSakEntityAsync()
    {
        var expectedSak = new Faker<Sak>().Generate()
            with
        {
            Organisajonsnummer = SampleOrgNr
        };
        
        // act
        var createdSak = await _sut.PersistSak(expectedSak);
        // assert
        var result = await _sut.GetSak(createdSak.Id);
        result.ShouldBeEquivalentTo(expectedSak);
    }

    [Fact]
    public async Task UpdateSakStatus_WhenCalled_PersistsSakEntityAsync()
    {
        // arrange
        var createdSak = new Faker<Sak>().Generate()
            with
        {
            Organisajonsnummer = SampleOrgNr
        };
        await _sut.PersistSak(createdSak);
        // act
        var updatedSak = await _sut.UpdateSakStatus(createdSak.Id, SakStatus.InProgress);
        // assert
        updatedSak.ShouldNotBeNull();
        updatedSak.ShouldBeEquivalentTo(
            createdSak with
            {
                Status = SakStatus.InProgress,
                LastUpdated = updatedSak.LastUpdated,
            }
        );
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

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Bogus;
using Shouldly;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class SakRepositoryTests : TestBed<InfrastructureAdapterTestFixture>
{
    private readonly ISakRepository _sut;

    private readonly Faker<SakEntity> _sakEntityFaker = TestData.CreateSakEntityFaker();

    private static readonly string SampleOrgNr = "123456789";

    public SakRepositoryTests(
        ITestOutputHelper testOutputHelper,
        InfrastructureAdapterTestFixture infrastractureAdapterTestFixture
    )
        : base(testOutputHelper, infrastractureAdapterTestFixture)
    {
        _sut = infrastractureAdapterTestFixture.GetService<ISakRepository>(testOutputHelper)!;
    }

    [Fact]
    public async Task CreateSak_WhenCalled_PersistsSakEntityAsync()
    {
        // act
        var createdSak = await _sut.PersistSak(SampleOrgNr);
        // assert
        var result = await _sut.GetSak(createdSak.Id);
        result?.Organisajonsnummer.ShouldBe(SampleOrgNr);
    }

    [Fact]
    public async Task UpdateSakStatus_WhenCalled_PersistsSakEntityAsync()
    {
        // arrange
        var createdSak = await _sut.PersistSak(SampleOrgNr);
        // act
        var updatedSak = await _sut.UpdateSakStatus(createdSak.Id, SakStatus.InProgress);
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
        var allSaker = await _sut.GetSaker();
        // assert
        seed.Select(sak => sak.Id).ToList().ShouldBeSubsetOf([.. allSaker.Select(sak => sak.Id)]);
    }
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Infrastructure;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Test.Fixtures;
using Shouldly;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Test;

public class SakRepositoryTests : TestBed<InfrastructureAdapterTestFixture>
{
    private readonly ISakRepository _sut; // System Under Test

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
        // arrange
        var newSak = TestData.CreateSakFaker(1).Generate() with
        {
            Organisasjonsnummer = SampleOrgNr,
        };

        // act
        var createdSak = await _sut.PersistSak(newSak);
        // assert
        var result = await _sut.GetSak(createdSak.Id);
        result.ShouldNotBeNull();
        result.Id.ShouldBe(newSak.Id);
        result.Organisasjonsnummer.ShouldBe(newSak.Organisasjonsnummer);
        result.CreatedAt.ShouldBe(createdSak.CreatedAt);
        result.Deadline.ShouldBe(createdSak.Deadline);
        result.LastUpdated.ShouldBe(createdSak.LastUpdated);
        result.Status.ShouldBe(newSak.Status);
    }

    [Fact]
    public async Task UpdateSakStatus_WhenCalled_PersistsSakEntityAsync()
    {
        // arrange
        var createdSak = TestData.CreateSakFaker(2).Generate() with
        {
            Organisasjonsnummer = SampleOrgNr,
        };
        var persistedSak = await _sut.PersistSak(createdSak);
        // act
        var updatedSak = await _sut.UpdateSakStatus(createdSak.Id, SakStatus.InProgress);
        // assert
        updatedSak.ShouldNotBeNull();
        updatedSak.Id.ShouldBe(createdSak.Id);
        updatedSak.Organisasjonsnummer.ShouldBe(createdSak.Organisasjonsnummer);
        updatedSak.CreatedAt.ShouldBe(persistedSak.CreatedAt);
        updatedSak.Deadline.ShouldBe(persistedSak.Deadline);
        updatedSak.LastUpdated.ShouldBeGreaterThan(persistedSak.LastUpdated);
        updatedSak.Status.ShouldBe(SakStatus.InProgress);
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

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Bogus;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.Test;

public class SakServiceTests
{
    private readonly TilsynssakService _sut; // System Under Test
    private readonly ITilsynssakRepository _tilsynssakRepositoryMock =
        Substitute.For<ITilsynssakRepository>();

    private const string SampleOrgNr = "123456789";

    private readonly DomainConfiguration _domainConfiguration = new() { SakDeadlineDays = 30 };

    public SakServiceTests()
    {
        _sut = new TilsynssakService(
            _tilsynssakRepositoryMock,
            Options.Create(_domainConfiguration)
        );
    }

    [Fact]
    public async Task CreateNewSak_WhenCalledWithCreateSakDto_ReturnsMockedSak()
    {
        //arrange
        var mockedSakResponse = new Faker<Tilsynssak>().Generate() with
        {
            Organisajonsnummer = SampleOrgNr,
        };
        _tilsynssakRepositoryMock.PersistSak(default!).ReturnsForAnyArgs(mockedSakResponse);
        //act
        var result = await _sut.CreateNewSak(
            new CreateTilsynssakDto { Organisajonsnummer = SampleOrgNr }
        );
        //assert
        result.ShouldBeEquivalentTo(mockedSakResponse);
    }

    [Fact]
    public async Task GetAllSaker_WhenCalled_ReturnsAllMockedSaker()
    {
        //arrange
        var mockedSakerResponse = new Faker<Tilsynssak>().Generate(10);
        _tilsynssakRepositoryMock.GetSaker().Returns(mockedSakerResponse);
        //act
        var result = await _sut.GetAllSaker();
        //assert
        result.ShouldBeEquivalentTo(mockedSakerResponse);
    }

    [Fact]
    public async Task GetSakById_WhenCalledWithExistingId_ReturnsMockedSak()
    {
        //arrange
        var testId = Guid.NewGuid();
        var mockedSakResponse = new Faker<Tilsynssak>().Generate() with { Id = testId };
        _tilsynssakRepositoryMock.GetSak(testId).Returns(mockedSakResponse);
        //act
        var result = await _sut.GetSakById(testId);
        //assert
        result.ShouldBeEquivalentTo(mockedSakResponse);
    }

    [Fact]
    public async Task GetSakById_WhenCalledWithNonExistingId_ThrowsAsync()
    {
        //arrange
        var testId = Guid.NewGuid();
        Tilsynssak? mockedSakResponse = null;
        _tilsynssakRepositoryMock.GetSak(testId).Returns(mockedSakResponse);
        //act
        var act = () => _sut.GetSakById(testId);
        //assert
        await act.ShouldThrowAsync<SakNotFoundException>();
    }

    [Fact]
    public async Task StartSak_WhenCalledWithExistingId_UpdatesSakAndReturnsMockedSak()
    {
        //arrange
        var testId = Guid.NewGuid();
        var mockedSakResponse = new Faker<Tilsynssak>().Generate() with { Id = testId };
        _tilsynssakRepositoryMock
            .UpdateSakStatus(testId, SakStatus.InProgress)
            .Returns(mockedSakResponse);
        //act
        var result = await _sut.StartSak(testId);
        //assert
        result.ShouldBeEquivalentTo(mockedSakResponse);
    }

    [Fact]
    public async Task StartSak_WhenCalledWithNonExistingId_ThrowsAsync()
    {
        //arrange
        var testId = Guid.NewGuid();
        _tilsynssakRepositoryMock
            .UpdateSakStatus(testId, SakStatus.InProgress)
            .Returns(default(Tilsynssak?));
        //act
        var act = () => _sut.StartSak(testId);
        //assert
        await act.ShouldThrowAsync<SakNotFoundException>();
    }

    [Fact]
    public async Task EndSak_WhenCalledWithExistingId_UpdatesSakAndReturnsMockedSak()
    {
        //arrange
        var testId = Guid.NewGuid();
        var mockedSakResponse = new Faker<Tilsynssak>().Generate() with { Id = testId };
        _tilsynssakRepositoryMock
            .UpdateSakStatus(testId, SakStatus.Done)
            .Returns(mockedSakResponse);
        //act
        var result = await _sut.EndSak(testId);
        //assert
        result.ShouldBeEquivalentTo(mockedSakResponse);
    }

    [Fact]
    public async Task EndSak_WhenCalledWithNonExistingId_ThrowsAsync()
    {
        //arrange
        var testId = Guid.NewGuid();
        Tilsynssak? mockedSakResponse = null;
        _tilsynssakRepositoryMock
            .UpdateSakStatus(testId, SakStatus.Done)
            .Returns(mockedSakResponse);
        //act
        var act = () => _sut.EndSak(testId);
        //assert
        await act.ShouldThrowAsync<SakNotFoundException>();
    }

    [Fact]
    public async Task ArchiveSak_WhenCalledWithExistingId_UpdatesSakAndReturnsMockedSak()
    {
        //arrange
        var testId = Guid.NewGuid();
        var mockedSakResponse = new Faker<Tilsynssak>().Generate() with { Id = testId };
        _tilsynssakRepositoryMock
            .UpdateSakStatus(testId, SakStatus.Archived)
            .Returns(mockedSakResponse);
        //act
        var result = await _sut.ArchiveSak(testId);
        //assert
        result.ShouldBeEquivalentTo(mockedSakResponse);
    }

    [Fact]
    public async Task ArchiveSak_WhenCalledWithNonExistingId_ThrowsAsync()
    {
        //arrange
        var testId = Guid.NewGuid();
        Tilsynssak? mockedSakResponse = null;
        _tilsynssakRepositoryMock
            .UpdateSakStatus(testId, SakStatus.Archived)
            .Returns(mockedSakResponse);
        //act
        var act = () => _sut.ArchiveSak(testId);
        //assert
        await act.ShouldThrowAsync<SakNotFoundException>();
    }
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.Exceptions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driven;
using Bogus;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.Test;

public class SakServiceTests
{
    private readonly IProcessSakEvents _sut;
    private readonly ISaveSaker _saveRepositoryMock = Substitute.For<ISaveSaker>();
    private readonly IGetSaker _getRepositoryMock = Substitute.For<IGetSaker>();

    private static readonly string SampleOrgNr = "123456789";

    private readonly DomainConfiguration _domainConfiguration = new()
    {
        SomeSetting = "SampleConfigValue",
    };

    public SakServiceTests()
    {
        _sut = new SakService(_saveRepositoryMock, _getRepositoryMock, Options.Create(_domainConfiguration));
    }

    [Fact]
    public async Task CreateNewSak_WhenCalledWithCreateSakDto_ReturnsMockedSak()
    {
        //arrange
        var mockedSakResponse = new Faker<Sak>().Generate() with
        {
            Organisajonsnummer = SampleOrgNr,
        };
        _saveRepositoryMock.PersistSak(SampleOrgNr).Returns(mockedSakResponse);
        //act
        var result = await _sut.CreateNewSak(new CreateSakDto { Organisajonsnummer = SampleOrgNr });
        //assert
        result.ShouldBeEquivalentTo(mockedSakResponse);
    }

    [Fact]
    public async Task GetAllSaker_WhenCalled_ReturnsAllMockedSaker()
    {
        //arrange
        var mockedSakerResponse = new Faker<Sak>().Generate(10);
        _getRepositoryMock.GetSaker().Returns(mockedSakerResponse);
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
        var mockedSakResponse = new Faker<Sak>().Generate() with { Id = testId };
        _getRepositoryMock.GetSak(testId).Returns(mockedSakResponse);
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
        Sak? mockedSakResponse = null;
        _getRepositoryMock.GetSak(testId).Returns(mockedSakResponse);
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
        var mockedSakResponse = new Faker<Sak>().Generate() with { Id = testId };
        _saveRepositoryMock.UpdateSakStatus(testId, SakStatus.InProgress).Returns(mockedSakResponse);
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
        Sak? mockedSakResponse = null;
        _saveRepositoryMock.UpdateSakStatus(testId, SakStatus.InProgress).Returns(mockedSakResponse);
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
        var mockedSakResponse = new Faker<Sak>().Generate() with { Id = testId };
        _saveRepositoryMock.UpdateSakStatus(testId, SakStatus.Done).Returns(mockedSakResponse);
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
        Sak? mockedSakResponse = null;
        _saveRepositoryMock.UpdateSakStatus(testId, SakStatus.Done).Returns(mockedSakResponse);
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
        var mockedSakResponse = new Faker<Sak>().Generate() with { Id = testId };
        _saveRepositoryMock.UpdateSakStatus(testId, SakStatus.Archived).Returns(mockedSakResponse);
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
        Sak? mockedSakResponse = null;
        _saveRepositoryMock.UpdateSakStatus(testId, SakStatus.Archived).Returns(mockedSakResponse);
        //act
        var act = () => _sut.ArchiveSak(testId);
        //assert
        await act.ShouldThrowAsync<SakNotFoundException>();
    }
}

using System.Net;
using System.Net.Http.Json;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.fixture;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test;

public class ActionsControllerIntegrationTests : IClassFixture<ApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _client;

    private Sak? TestSak;

    public ActionsControllerIntegrationTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ActionsStartSak_Post_UpdatesStatus()
    {
        // Act
        var response = await _client.PostAsync($"/actions/start-sak?sakId={TestSak!.Id}", null);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Sak>();
        result?.ShouldBeEquivalentTo(
            TestSak with
            {
                Status = SakStatus.InProgress,
                LastUpdated = result.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task ActionsStartSak_PostWithNotExistingId_Returns400()
    {
        // Act
        var response = await _client.PostAsync($"/actions/start-sak?sakId={Guid.NewGuid}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsStartSak_PostWithInvalidId_Returns400()
    {
        // Act
        var response = await _client.PostAsync("/actions/start-sak?sakId=111", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsEndSak_Post_UpdatesStatus()
    {
        // Act
        var response = await _client.PostAsync($"/actions/end-sak?sakId={TestSak!.Id}", null);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Sak>();
        result?.ShouldBeEquivalentTo(
            TestSak with
            {
                Status = SakStatus.Done,
                LastUpdated = result.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task ActionsEndSak_PostWithNotExistingId_Returns400()
    {
        // Act
        var response = await _client.PostAsync($"/actions/end-sak?sakId={Guid.NewGuid}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsEndSak_PostWithInvalidId_Returns400()
    {
        // Act
        var response = await _client.PostAsync("/actions/end-sak?sakId=111", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsArchiveSak_Post_UpdatesStatus()
    {
        // Act
        var response = await _client.PostAsync($"/actions/archive-sak?sakId={TestSak!.Id}", null);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Sak>();
        result?.ShouldBeEquivalentTo(
            TestSak with
            {
                Status = SakStatus.Archived,
                LastUpdated = result.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task ActionsArchiveSak_PostWithNotExistingId_Returns400()
    {
        // Act
        var response = await _client.PostAsync($"/actions/archive-sak?sakId={Guid.NewGuid}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsArchiveSak_PostWithInvalidId_Returns400()
    {
        // Act
        var response = await _client.PostAsync("/actions/archive-sak?sakId=111", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    public async Task InitializeAsync()
    {
        var response = await _client.PostAsJsonAsync(
            "/saker",
            new CreateSakDto { Organisajonsnummer = "123456789" }
        );

        // Assert
        TestSak = await response.Content.ReadFromJsonAsync<Sak>();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

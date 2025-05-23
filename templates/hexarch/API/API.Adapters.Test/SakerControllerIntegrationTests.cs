using System.Net;
using System.Net.Http.Json;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.fixture;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test;

public class SakerControllerIntegrationTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public SakerControllerIntegrationTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ScalarEndpoint_ReturnsOK()
    {
        // Act
        var response = await _client.GetAsync("/scalar/v1");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Saker_Post_CreatesNewSak()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/saker",
            new CreateSakDto { Organisajonsnummer = "123456789" }
        );

        // Assert
        (await response.Content.ReadFromJsonAsync<Sak>())?.Organisajonsnummer.ShouldBe("123456789");
    }

    [Fact]
    public async Task Saker_PostWithInvalidOrgNr_Returns400()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/saker",
            new CreateSakDto { Organisajonsnummer = "111" }
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Saker_Get_ReturnsOK()
    {
        // Act
        var response = await _client.GetAsync("/saker");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Saker_GetById_ReturnsSak()
    {
        // Arrange
        var createdSak = await (
            await _client.PostAsJsonAsync(
                "/saker",
                new CreateSakDto { Organisajonsnummer = "123456789" }
            )
        ).Content.ReadFromJsonAsync<Sak>();
        // Act
        var response = await _client.GetFromJsonAsync<Sak>($"/saker/{createdSak!.Id}");

        // Assert
        response.ShouldBeEquivalentTo(createdSak);
    }

    [Fact]
    public async Task Saker_GetByNotExistingId_Returns404()
    {
        // Act
        var response = await _client.GetAsync($"/saker/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Saker_GetByMalformedId_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/saker/234}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}

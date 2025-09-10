using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.Fixture;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test;

public class SakerControllerIntegrationTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;

    public SakerControllerIntegrationTests(ApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        _options.Converters.Add(new JsonStringEnumConverter());
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
        const string orgNr = "987654321";

        // Act
        var response = await _client.PostAsJsonAsync(
            "/saker",
            new CreateSakDto { Organisajonsnummer = orgNr }
        );

        // Assert
        (await response.Content.ReadFromJsonAsync<Sak>(_options))?.Organisajonsnummer.ShouldBe(
            orgNr
        );
    }

    [Theory]
    [InlineData("123")] // Too short
    [InlineData("1234567890")] // Too long
    [InlineData("abcdefghi")] // Not numeric
    public async Task Saker_PostWithInvalidOrgNr_Returns400(string invalidOrgnummer)
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/saker",
            new CreateSakDto { Organisajonsnummer = invalidOrgnummer }
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
        ).Content.ReadFromJsonAsync<Sak>(_options);
        // Act
        var response = await _client.GetFromJsonAsync<Sak>($"/saker/{createdSak!.Id}", _options);

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

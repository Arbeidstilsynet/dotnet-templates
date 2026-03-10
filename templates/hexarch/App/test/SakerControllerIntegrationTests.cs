using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Test.Fixture;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App.Requests;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.Test;

[Collection("Application Fixture Collection")]
public class SakerControllerIntegrationTests(ApplicationFixture fixture)
{
    private readonly HttpClient _client = fixture.CreateClient();
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    [Fact]
    public async Task ScalarEndpoint_ReturnsOK()
    {
        // Act
        var response = await _client.GetAsync("/scalar/v1", TestContext.Current.CancellationToken);

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
            new CreateSakDto { Organisasjonsnummer = orgNr },
            TestContext.Current.CancellationToken
        );

        // Assert
        (
            await response.Content.ReadFromJsonAsync<Sak>(
                _options,
                TestContext.Current.CancellationToken
            )
        )?.Organisasjonsnummer.ShouldBe(orgNr);
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
            new CreateSakDto { Organisasjonsnummer = invalidOrgnummer },
            TestContext.Current.CancellationToken
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Saker_Get_ReturnsOK()
    {
        // Act
        var response = await _client.GetAsync("/saker", TestContext.Current.CancellationToken);

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
                new CreateSakDto { Organisasjonsnummer = "123456789" },
                TestContext.Current.CancellationToken
            )
        ).Content.ReadFromJsonAsync<Sak>(_options, TestContext.Current.CancellationToken);
        // Act
        var response = await _client.GetFromJsonAsync<Sak>(
            $"/saker/{createdSak!.Id}",
            _options,
            TestContext.Current.CancellationToken
        );

        // Assert
        response.ShouldBeEquivalentTo(createdSak);
    }

    [Fact]
    public async Task Saker_GetByNotExistingId_Returns404()
    {
        // Act
        var response = await _client.GetAsync(
            $"/saker/{Guid.NewGuid()}",
            TestContext.Current.CancellationToken
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Saker_GetByMalformedId_Returns404()
    {
        // Act
        var response = await _client.GetAsync("/saker/234}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test.Fixture;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Shouldly;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.Test;

public class ActionsControllerIntegrationTests(ApplicationFixture fixture)
    : IClassFixture<ApplicationFixture>
{
    private readonly HttpClient _client = fixture.CreateClient();
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    [Fact]
    public async Task ActionsStartSak_Post_UpdatesStatus()
    {
        // Act
        var response = await _client.PostAsync(
            $"/actions/start-sak?sakId={fixture.SeededSak.Id}",
            null
        );

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Sak>(_options);
        result?.ShouldBeEquivalentTo(
            fixture.SeededSak with
            {
                Status = SakStatus.InProgress,
                LastUpdated = result.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task ActionsStartSak_PostWithNotExistingId_Returns404()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/actions/start-sak?sakId={nonExistentId}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-guid")]
    [InlineData("12345")]
    [InlineData("00000000-0000-0000-0000-00000000000G")] // Invalid character 'G'
    public async Task ActionsStartSak_PostWithInvalidId_Returns400(string invalidId)
    {
        // Act
        var response = await _client.PostAsync($"/actions/start-sak?sakId={invalidId}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsEndSak_Post_UpdatesStatus()
    {
        // Act
        var response = await _client.PostAsync(
            $"/actions/end-sak?sakId={fixture.SeededSak.Id}",
            null
        );

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Sak>(_options);
        result?.ShouldBeEquivalentTo(
            fixture.SeededSak with
            {
                Status = SakStatus.Done,
                LastUpdated = result.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task ActionsEndSak_PostWithNotExistingId_Returns404()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/actions/end-sak?sakId={nonExistentId}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-guid")]
    [InlineData("12345")]
    [InlineData("00000000-0000-0000-0000-00000000000G")] // Invalid character 'G'
    public async Task ActionsEndSak_PostWithInvalidId_Returns400(string invalidId)
    {
        // Act
        var response = await _client.PostAsync($"/actions/end-sak?sakId={invalidId}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ActionsArchiveSak_Post_UpdatesStatus()
    {
        // Act
        var response = await _client.PostAsync(
            $"/actions/archive-sak?sakId={fixture.SeededSak.Id}",
            null
        );

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Sak>(_options);
        result?.ShouldBeEquivalentTo(
            fixture.SeededSak with
            {
                Status = SakStatus.Archived,
                LastUpdated = result.LastUpdated,
            }
        );
    }

    [Fact]
    public async Task ActionsArchiveSak_PostWithNotExistingId_Returns404()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/actions/archive-sak?sakId={nonExistentId}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-guid")]
    [InlineData("12345")]
    [InlineData("00000000-0000-0000-0000-00000000000G")] // Invalid character 'G'
    public async Task ActionsArchiveSak_PostWithInvalidId_Returns400(string invalidId)
    {
        // Act
        var response = await _client.PostAsync($"/actions/archive-sak?sakId={invalidId}", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}

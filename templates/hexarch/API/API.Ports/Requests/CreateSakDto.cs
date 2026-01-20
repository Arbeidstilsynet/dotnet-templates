using System.ComponentModel.DataAnnotations;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;

public record CreateSakDto
{
    [RegularExpression(@"^\d{9}$")]
    public required string Organisasjonsnummer { get; init; }
}

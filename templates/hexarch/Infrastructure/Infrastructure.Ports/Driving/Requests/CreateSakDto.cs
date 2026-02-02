using System.ComponentModel.DataAnnotations;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving.Requests;

public record CreateSakDto
{
    [RegularExpression(@"^\d{9}$")]
    public required string Organisajonsnummer { get; init; }
}

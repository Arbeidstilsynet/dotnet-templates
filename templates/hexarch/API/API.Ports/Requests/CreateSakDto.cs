using System.ComponentModel.DataAnnotations;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;

/// <summary>
/// Data Transfer Object for creating a new Sak.
/// </summary>
public record CreateSakDto
{
    /// <summary>
    /// A reference to the organization number associated with the case.
    /// </summary>
    [Required]
    [RegularExpression(@"^\d{9}$")]
    public required string Organisasjonsnummer { get; init; }
}

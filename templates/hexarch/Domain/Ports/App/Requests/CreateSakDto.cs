using System.ComponentModel.DataAnnotations;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App.Requests;

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

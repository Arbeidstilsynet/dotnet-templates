namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

/// <summary>
/// Represents a case in this domain.
/// </summary>
public record Tilsynssak
{
    /// <summary>
    /// Unique identifier for the case.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The organization number associated with the case. It refers to a Norwegian organization.
    /// </summary>
    public required string Organisajonsnummer { get; init; }
    
    /// <summary>
    /// The timestamp of case creation.
    /// </summary>
    public required DateTime CreatedAt { get; init; }
    
    /// <summary>
    /// The deadline for finishing the case.
    /// </summary>
    public required DateTime Deadline { get; init; }

    /// <summary>
    /// The timestamp of the last update.
    /// </summary>
    public required DateTime LastUpdated { get; init; }

    /// <summary>
    /// The current status of the case.
    /// </summary>
    public required SakStatus Status { get; init; }
}

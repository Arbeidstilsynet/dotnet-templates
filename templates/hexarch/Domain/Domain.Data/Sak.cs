namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

public record Sak
{
    public required Guid Id { get; init; }

    public required string Organisajonsnummer { get; init; }

    public required DateTime CreatedAt { get; init; }

    public required DateTime LastUpdated { get; init; }

    public required SakStatus Status { get; init; }
}

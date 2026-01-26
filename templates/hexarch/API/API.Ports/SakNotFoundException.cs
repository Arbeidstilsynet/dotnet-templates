namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

/// <summary>
/// Exception thrown when a <see cref="Domain.Data.Sak"/> with the specified ID is not found.
/// </summary>
/// <param name="sakId"></param>
public class SakNotFoundException(Guid sakId)
    : Exception(
        message: $"Could not find sak with SakId {sakId}",
        innerException: new InvalidOperationException()
    ) { }

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

public class SakNotFoundException(Guid sakId)
    : Exception(
        message: $"Could not find sak with SakId {sakId}",
        innerException: new InvalidOperationException()
    ) { }

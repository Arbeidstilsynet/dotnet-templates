namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

/// <summary>
/// Enumerates the possible statuses of a case <see cref="Tilsynssak"/>.
/// </summary>
public enum SakStatus
{
    /// <summary>
    /// The case is newly created and has not yet been started.
    /// </summary>
    New,

    /// <summary>
    /// The case is currently in progress. Work is ongoing.
    /// </summary>
    InProgress,

    /// <summary>
    /// The case has been completed. It has to be done by the deadline.
    /// </summary>
    Done,

    /// <summary>
    /// The case has been archived and is no longer active.
    /// </summary>
    Archived,
}

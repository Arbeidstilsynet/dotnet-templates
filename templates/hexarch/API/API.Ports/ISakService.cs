using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

public interface ISakService
{
    /// <summary>
    /// Creates a new <see cref="Sak"/> based on the provided DTO. The deadline is set based on the domain configuration.
    /// </summary>
    /// <param name="sakDto"></param>
    /// <returns></returns>
    public Task<Sak> CreateNewSak(CreateSakDto sakDto);

    /// <summary>
    /// Starts the case with the given <paramref name="sakId"/>, changing its status to <see cref="SakStatus.InProgress"/>.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Sak> StartSak(Guid sakId);

    /// <summary>
    /// Ends the case with the given <paramref name="sakId"/>, changing its status to <see cref="SakStatus.Done"/>.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Sak> EndSak(Guid sakId);

    /// <summary>
    /// Archives the case with the given <paramref name="sakId"/>, changing its status to <see cref="SakStatus.Archived"/>.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Sak> ArchiveSak(Guid sakId);

    /// <summary>
    /// Retrieves all <see cref="Sak"/>.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Sak>> GetAllSaker();

    /// <summary>
    /// Retrieves a <see cref="Sak"/> by its unique identifier.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Sak> GetSakById(Guid sakId);
}

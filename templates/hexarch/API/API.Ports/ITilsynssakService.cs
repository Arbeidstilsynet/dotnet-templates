using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

public interface ITilsynssakService
{
    /// <summary>
    /// Creates a new Tilsynssak based on the provided DTO. The deadline is set based on the domain configuration.
    /// </summary>
    /// <param name="tilsynssakDto"></param>
    /// <returns></returns>
    public Task<Tilsynssak> CreateNewSak(CreateTilsynssakDto tilsynssakDto);
    
    /// <summary>
    /// Starts the case with the given <paramref name="sakId"/>, changing its status to <see cref="SakStatus.InProgress"/>.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Tilsynssak> StartSak(Guid sakId);

    
    /// <summary>
    /// Ends the case with the given <paramref name="sakId"/>, changing its status to <see cref="SakStatus.Done"/>.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Tilsynssak> EndSak(Guid sakId);
    
    /// <summary>
    /// Archives the case with the given <paramref name="sakId"/>, changing its status to <see cref="SakStatus.Archived"/>.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Tilsynssak> ArchiveSak(Guid sakId);

    /// <summary>
    /// Retrieves all Tilsynssak cases.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Tilsynssak>> GetAllSaker();

    /// <summary>
    /// Retrieves a Tilsynssak by its unique identifier.
    /// </summary>
    /// <param name="sakId"></param>
    /// <returns></returns>
    public Task<Tilsynssak> GetSakById(Guid sakId);
}

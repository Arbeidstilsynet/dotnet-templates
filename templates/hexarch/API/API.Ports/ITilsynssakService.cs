using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

public interface ITilsynssakService
{
    public Task<Tilsynssak> CreateNewSak(CreateTilsynssakDto tilsynssakDto);

    public Task<Tilsynssak> StartSak(Guid sakId);

    public Task<Tilsynssak> EndSak(Guid sakId);
    public Task<Tilsynssak> ArchiveSak(Guid sakId);

    public Task<IEnumerable<Tilsynssak>> GetAllSaker();

    public Task<Tilsynssak> GetSakById(Guid sakId);
}

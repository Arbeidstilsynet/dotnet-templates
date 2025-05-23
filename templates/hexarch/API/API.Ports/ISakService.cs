using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;

public interface ISakService
{
    public Task<Sak> CreateNewSak(CreateSakDto sakDto);

    public Task<Sak> StartSak(Guid sakId);

    public Task<Sak> EndSak(Guid sakId);
    public Task<Sak> ArchiveSak(Guid sakId);

    public Task<IEnumerable<Sak>> GetAllSaker();

    public Task<Sak> GetSakById(Guid sakId);
}

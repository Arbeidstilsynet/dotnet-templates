using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving.Requests;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving;

public interface IProcessSakEvents
{
    public Task<Sak> GetSakById(Guid sakId);
    public Task<IEnumerable<Sak>> GetAllSaker();
    public Task<Sak> CreateNewSak(CreateSakDto sakDto);

    public Task<Sak> StartSak(Guid sakId);

    public Task<Sak> EndSak(Guid sakId);
    public Task<Sak> ArchiveSak(Guid sakId);
}

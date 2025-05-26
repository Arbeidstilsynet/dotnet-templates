using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

internal class SakService(ISakRepository _sakRepository) : ISakService
{
    public async Task<Sak> CreateNewSak(CreateSakDto sakDto)
    {
        return await _sakRepository.CreateSak(sakDto.Organisajonsnummer);
    }

    public async Task<IEnumerable<Sak>> GetAllSaker()
    {
        return await _sakRepository.GetSaker();
    }

    public async Task<Sak> ArchiveSak(Guid sakId)
    {
        return await _sakRepository.UpdateSakStatus(sakId, SakStatus.Archived)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> EndSak(Guid sakId)
    {
        return await _sakRepository.UpdateSakStatus(sakId, SakStatus.Done)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> StartSak(Guid sakId)
    {
        return await _sakRepository.UpdateSakStatus(sakId, SakStatus.InPrograss)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> GetSakById(Guid sakId)
    {
        return await _sakRepository.GetSak(sakId) ?? throw new SakNotFoundException(sakId);
    }
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.Exceptions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driving.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Driven;
using Microsoft.Extensions.Options;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

internal class SakService(
    ISaveSaker saveRepository,
    IGetSaker getRepository,
    IOptions<DomainConfiguration> domainConfig)
    : IProcessSakEvents
{
    private readonly string _someConfigValue = domainConfig.Value.SomeSetting;

    public async Task<Sak> CreateNewSak(CreateSakDto sakDto)
    {
        using var activity = Tracer.Source.StartActivity();
        return await saveRepository.PersistSak(sakDto.Organisajonsnummer);
    }

    public async Task<IEnumerable<Sak>> GetAllSaker()
    {
        return await getRepository.GetSaker();
    }

    public async Task<Sak> ArchiveSak(Guid sakId)
    {
        return await saveRepository.UpdateSakStatus(sakId, SakStatus.Archived) ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> EndSak(Guid sakId)
    {
        return await saveRepository.UpdateSakStatus(sakId, SakStatus.Done) ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> StartSak(Guid sakId)
    {
        return await saveRepository.UpdateSakStatus(sakId, SakStatus.InProgress) ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> GetSakById(Guid sakId)
    {
        return await getRepository.GetSak(sakId) ?? throw new SakNotFoundException(sakId);
    }
}

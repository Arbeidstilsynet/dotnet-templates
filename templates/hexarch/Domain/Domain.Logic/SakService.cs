using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Microsoft.Extensions.Options;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

internal class SakService(ISakRepository sakRepository, IOptions<DomainConfiguration> domainConfig)
    : ISakService
{
    public async Task<Sak> CreateNewSak(CreateSakDto sakDto)
    {
        using var activity = Tracer.Source.StartActivity();
        
        var createdAt = DateTime.UtcNow;
        var deadline = createdAt + new TimeSpan(domainConfig.Value.SakDeadlineDays);

        var sak = new Sak
        {
            Id =  Guid.NewGuid(),
            CreatedAt = createdAt,
            LastUpdated = createdAt,
            Organisajonsnummer = sakDto.Organisajonsnummer,
            Status = SakStatus.New,
            Deadline = deadline,
        };
        
        return await sakRepository.PersistSak(sak);
    }

    public async Task<IEnumerable<Sak>> GetAllSaker()
    {
        return await sakRepository.GetSaker();
    }

    public async Task<Sak> ArchiveSak(Guid sakId)
    {
        return await sakRepository.UpdateSakStatus(sakId, SakStatus.Archived)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> EndSak(Guid sakId)
    {
        return await sakRepository.UpdateSakStatus(sakId, SakStatus.Done)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> StartSak(Guid sakId)
    {
        return await sakRepository.UpdateSakStatus(sakId, SakStatus.InProgress)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Sak> GetSakById(Guid sakId)
    {
        return await sakRepository.GetSak(sakId) ?? throw new SakNotFoundException(sakId);
    }
}

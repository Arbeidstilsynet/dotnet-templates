using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.Requests;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.DependencyInjection;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;
using Microsoft.Extensions.Options;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

internal class TilsynssakService(
    ITilsynssakRepository tilsynssakRepository,
    IOptions<DomainConfiguration> domainConfig
) : ITilsynssakService
{
    public async Task<Tilsynssak> CreateNewSak(CreateTilsynssakDto tilsynssakDto)
    {
        using var activity = Tracer.Source.StartActivity();

        var createdAt = DateTime.UtcNow;
        var deadline = createdAt + new TimeSpan(domainConfig.Value.SakDeadlineDays);

        var sak = new Tilsynssak
        {
            Id = Guid.NewGuid(),
            CreatedAt = createdAt,
            LastUpdated = createdAt,
            Organisajonsnummer = tilsynssakDto.Organisajonsnummer,
            Status = SakStatus.New,
            Deadline = deadline,
        };

        return await tilsynssakRepository.PersistSak(sak);
    }

    public async Task<IEnumerable<Tilsynssak>> GetAllSaker()
    {
        return await tilsynssakRepository.GetSaker();
    }

    public async Task<Tilsynssak> ArchiveSak(Guid sakId)
    {
        return await tilsynssakRepository.UpdateSakStatus(sakId, SakStatus.Archived)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Tilsynssak> EndSak(Guid sakId)
    {
        return await tilsynssakRepository.UpdateSakStatus(sakId, SakStatus.Done)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Tilsynssak> StartSak(Guid sakId)
    {
        return await tilsynssakRepository.UpdateSakStatus(sakId, SakStatus.InProgress)
            ?? throw new SakNotFoundException(sakId);
    }

    public async Task<Tilsynssak> GetSakById(Guid sakId)
    {
        return await tilsynssakRepository.GetSak(sakId) ?? throw new SakNotFoundException(sakId);
    }
}

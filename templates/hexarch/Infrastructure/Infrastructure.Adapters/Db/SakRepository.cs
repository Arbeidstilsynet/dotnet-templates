using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SakEntity = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model.SakEntity;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;

internal class SakRepository(
    InfrastructureAdaptersDbContext _dbContext,
    IMapper _mapper,
    ILogger<SakRepository> _logger
) : Infrastructure.Ports.ISakRepository
{
    private InfrastructureAdaptersDbContext DbContext
    {
        get
        {
            _dbContext.Database.EnsureCreated();
            return _dbContext;
        }
    }

    public async Task<Sak> CreateSak(string organisajonsnummer)
    {
        using var activity = Tracer.Source.StartActivity("persist SakEntity");
        var sakEntity = new SakEntity
        {
            Id = Guid.NewGuid(),
            Organisajonsnummer = organisajonsnummer,
            Status = SakStatus.New,
        };
        var updatedEntity = await DbContext.Saker.AddAsync(sakEntity);

        await DbContext.SaveChangesAsync();
        await updatedEntity.ReloadAsync();

        return _mapper.Map<Sak>(updatedEntity.Entity);
    }

    public async Task<Sak?> UpdateSakStatus(Guid id, SakStatus sakStatus)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            entity.Status = sakStatus;
            await DbContext.SaveChangesAsync();
            return _mapper.Map<Sak>(entity);
        }
        _logger.LogSakNotFound(id);
        return null;
    }

    public async Task<Sak?> GetSak(Guid? id)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            return _mapper.Map<Sak>(entity);
        }

        _logger.LogSakNotFound(id);
        return null;
    }

    public async Task<IEnumerable<Sak>> GetSaker()
    {
        return await DbContext.Saker.Select(b => _mapper.Map<Sak>(b)).ToListAsync();
    }
}

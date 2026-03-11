using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db;

internal class SakRepository(SakDbContext dbContext, IMapper mapper, ILogger<SakRepository> logger)
    : Domain.Ports.Infrastructure.ISakRepository
{
    private SakDbContext DbContext
    {
        get
        {
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }

    public async Task<Sak> PersistSak(Sak sak)
    {
        using var activity = Tracer.Source.StartActivity();
        var sakEntity = mapper.Map<SakEntity>(sak);
        var updatedEntity = await DbContext.Saker.AddAsync(sakEntity);

        await DbContext.SaveChangesAsync();
        await updatedEntity.ReloadAsync();

        return mapper.Map<Sak>(updatedEntity.Entity);
    }

    public async Task<Sak?> UpdateSakStatus(Guid id, SakStatus sakStatus)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            entity.Status = sakStatus;
            await DbContext.SaveChangesAsync();
            return mapper.Map<Sak>(entity);
        }
        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<Sak?> GetSak(Guid? id)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            return mapper.Map<Sak>(entity);
        }

        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<IEnumerable<Sak>> GetSaker()
    {
        return await DbContext.Saker.Select(b => mapper.Map<Sak>(b)).ToListAsync();
    }
}

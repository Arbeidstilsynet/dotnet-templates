using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;

internal class TilsynssakRepository(TilsynssakDbContext dbContext, IMapper mapper, ILogger<TilsynssakRepository> logger)
    : Ports.ITilsynssakRepository
{
    private TilsynssakDbContext DbContext
    {
        get
        {
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }

    public async Task<Tilsynssak> PersistSak(Tilsynssak tilsynssak)
    {
        using var activity = Tracer.Source.StartActivity();
        var sakEntity = mapper.Map<TilsynssakEntity>(tilsynssak);
        var updatedEntity = await DbContext.Saker.AddAsync(sakEntity);

        await DbContext.SaveChangesAsync();
        await updatedEntity.ReloadAsync();

        return mapper.Map<Tilsynssak>(updatedEntity.Entity);
    }

    public async Task<Tilsynssak?> UpdateSakStatus(Guid id, SakStatus sakStatus)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            entity.Status = sakStatus;
            await DbContext.SaveChangesAsync();
            return mapper.Map<Tilsynssak>(entity);
        }
        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<Tilsynssak?> GetSak(Guid? id)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            return mapper.Map<Tilsynssak>(entity);
        }

        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<IEnumerable<Tilsynssak>> GetSaker()
    {
        return await DbContext.Saker.Select(b => mapper.Map<Tilsynssak>(b)).ToListAsync();
    }
}

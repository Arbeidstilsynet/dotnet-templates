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

    public async Task<Sak> PersistSak(Sak sak)
    {
        using var activity = Tracer.Source.StartActivity();
        var sakEntity = mapper.Map<SakEntity>(sak);
        var updatedEntity = await dbContext.Saker.AddAsync(sakEntity);

        await dbContext.SaveChangesAsync();
        await updatedEntity.ReloadAsync();

        return mapper.Map<Sak>(updatedEntity.Entity);
    }

    public async Task<Sak?> UpdateSakStatus(Guid id, SakStatus sakStatus)
    {
        var entity = await dbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            entity.Status = sakStatus;
            await dbContext.SaveChangesAsync();
            return mapper.Map<Sak>(entity);
        }
        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<Sak?> GetSak(Guid? id)
    {
        var entity = await dbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            return mapper.Map<Sak>(entity);
        }

        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<IEnumerable<Sak>> GetSaker()
    {
        return await dbContext.Saker.Select(b => mapper.Map<Sak>(b)).ToListAsync();
    }
}

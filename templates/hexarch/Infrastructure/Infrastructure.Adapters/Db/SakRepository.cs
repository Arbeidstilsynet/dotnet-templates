using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Extensions;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SakEntity = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model.SakEntity;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;

internal class SakRepository(SakDbContext dbContext, ILogger<SakRepository> logger)
    : Ports.ISakRepository
{
    private SakDbContext DbContext
    {
        get
        {
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }

    public async Task<Sak> PersistSak(string organisajonsnummer)
    {
        using var activity = Tracer.Source.StartActivity();
        var sakEntity = new SakEntity
        {
            Id = Guid.NewGuid(),
            Organisajonsnummer = organisajonsnummer,
            Status = SakStatus.New,
        };
        var updatedEntity = await DbContext.Saker.AddAsync(sakEntity);

        await DbContext.SaveChangesAsync();
        await updatedEntity.ReloadAsync();

        return updatedEntity.Entity.ToDomain();
    }

    public async Task<Sak?> UpdateSakStatus(Guid id, SakStatus sakStatus)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            entity.Status = sakStatus;
            await DbContext.SaveChangesAsync();
            return entity.ToDomain();
        }
        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<Sak?> GetSak(Guid? id)
    {
        var entity = await DbContext.Saker.FindAsync(id);
        if (entity != null)
        {
            return entity.ToDomain();
        }

        logger.LogSakNotFound(id);
        return null;
    }

    public async Task<IEnumerable<Sak>> GetSaker()
    {
        return await DbContext.Saker.Select(b => b.ToDomain()).ToListAsync();
    }
}

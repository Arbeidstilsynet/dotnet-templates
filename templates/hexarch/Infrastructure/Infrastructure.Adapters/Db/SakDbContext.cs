using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db;

internal class SakDbContext(DbContextOptions<SakDbContext> dbContextOption)
    : DbContext(dbContextOption)
{
    public DbSet<SakEntity> Saker { get; init; }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker
            .Entries()
            .Where(x =>
                x.Entity is BaseEntity
                && (x.State == EntityState.Added || x.State == EntityState.Modified)
            );

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }
            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
}

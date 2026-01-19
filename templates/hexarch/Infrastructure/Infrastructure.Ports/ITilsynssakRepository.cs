using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports;

public interface ITilsynssakRepository
{
    public Task<Tilsynssak> PersistSak(Tilsynssak tilsynssak);

    public Task<Tilsynssak?> UpdateSakStatus(Guid id, SakStatus sakStatus);
    public Task<Tilsynssak?> GetSak(Guid? id);

    public Task<IEnumerable<Tilsynssak>> GetSaker();
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports.Driven;

public interface ISaveSaker
{
    public Task<Sak> PersistSak(string organisajonsnummer);
    public Task<Sak?> UpdateSakStatus(Guid id, SakStatus sakStatus);
}

public interface IGetSaker
{
    public Task<Sak?> GetSak(Guid? id);
    public Task<IEnumerable<Sak>> GetSaker();
}

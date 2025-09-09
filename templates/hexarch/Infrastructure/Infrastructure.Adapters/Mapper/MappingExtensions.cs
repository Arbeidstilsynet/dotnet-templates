using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Mapper;

internal static class MappingExtensions
{
    public static Sak ToDomain(this SakEntity entity) => new()
    {
        Id = entity.Id,
        CreatedAt = entity.CreatedAt,
        LastUpdated = entity.UpdatedAt,
        Organisajonsnummer = entity.Organisajonsnummer,
        Status = entity.Status
    };
}
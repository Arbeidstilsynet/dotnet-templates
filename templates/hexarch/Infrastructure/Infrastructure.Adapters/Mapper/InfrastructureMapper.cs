using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Mapster;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Mapper;

internal class InfrastructureMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<TilsynssakEntity, Tilsynssak>()
            .NameMatchingStrategy(NameMatchingStrategy.Flexible)
            .TwoWays()
            .Map(target => target.LastUpdated, source => source.UpdatedAt);
    }
}

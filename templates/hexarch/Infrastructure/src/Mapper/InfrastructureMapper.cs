using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db.Model;
using Mapster;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Mapper;

internal class InfrastructureMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<SakEntity, Sak>()
            .NameMatchingStrategy(NameMatchingStrategy.Flexible)
            .TwoWays()
            .Map(target => target.LastUpdated, source => source.UpdatedAt);
    }
}

using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Mapper;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Bogus;
using Shouldly;
using SakEntity = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model.SakEntity;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class SakEntityMapperTests
{
    private readonly Faker<SakEntity> _sakEntityFaker = TestData.CreateSakEntityFaker();

    [Fact]
    public void Map_SakEntity_To_Sak()
    {
        //arrange
        var sakEntity = _sakEntityFaker.Generate();

        //act
        var mappedSak = sakEntity.ToDomain();

        //assert
        mappedSak.ShouldBeEquivalentTo(
            new Sak()
            {
                Id = sakEntity.Id,
                Organisajonsnummer = sakEntity.Organisajonsnummer,
                Status = sakEntity.Status,
                CreatedAt = sakEntity.CreatedAt,
                LastUpdated = sakEntity.UpdatedAt,
            }
        );
    }
}

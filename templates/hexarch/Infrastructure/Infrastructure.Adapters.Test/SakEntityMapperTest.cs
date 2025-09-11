using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Mapper;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using Bogus;
using MapsterMapper;
using Shouldly;
using SakEntity = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model.SakEntity;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class InfrastructureMapperTests
{
    private readonly Faker<SakEntity> _sakEntityFaker = TestData.CreateSakEntityFaker();

    private readonly InfrastructureMapper _sut = new();
    private readonly MapsterMapper.Mapper _mapper = new();

    public InfrastructureMapperTests()
    {
        _sut.Register(_mapper.Config);
    }

    [Fact]
    public void Map_SakEntity_To_Sak()
    {
        //arrange
        var sakEntity = _sakEntityFaker.Generate();

        //act
        var mappedSak = _mapper.Map<Sak>(sakEntity);

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

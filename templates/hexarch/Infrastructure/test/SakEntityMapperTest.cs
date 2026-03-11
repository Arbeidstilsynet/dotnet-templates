using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db.Model;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Mapper;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Test.Fixtures;
using Bogus;
using Shouldly;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Test;

public class InfrastructureMapperTests
{
    private readonly Faker<SakEntity> _sakEntityFaker = TestData.CreateSakEntityFaker();

    private readonly InfrastructureMapper _sut = new(); // System Under Test
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
                Deadline = sakEntity.Deadline,
                Organisasjonsnummer = sakEntity.Organisasjonsnummer,
                Status = sakEntity.Status,
                CreatedAt = sakEntity.CreatedAt,
                LastUpdated = sakEntity.UpdatedAt,
            }
        );
    }
}

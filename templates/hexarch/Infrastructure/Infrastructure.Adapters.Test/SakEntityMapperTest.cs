using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;
using MapsterMapper;
using Shouldly;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using SakEntity = Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model.SakEntity;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test;

public class SakEntityMapperTests : TestBed<InfrastructureAdapterTestFixture>
{
    private readonly IMapper _mapper;

    public SakEntityMapperTests(
        ITestOutputHelper testOutputHelper,
        InfrastructureAdapterTestFixture applicationTestFixture
    )
        : base(testOutputHelper, applicationTestFixture)
    {
        _mapper = applicationTestFixture.GetService<IMapper>(testOutputHelper)!;
    }

    [Fact]
    public void Map_SakEntity_To_Sak()
    {
        //arrange
        var sakEntity = new SakEntity()
        {
            Id = Guid.NewGuid(),
            Organisajonsnummer = "123456789",
            Status = SakStatus.New,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
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

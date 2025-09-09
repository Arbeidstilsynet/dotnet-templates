using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Bogus;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;

internal static class TestData
{
    public static Faker<SakEntity> CreateSakEntityFaker() =>
        new Faker<SakEntity>()
            .UseSeed(1337)
            .RuleFor(sak => sak.Id, f => f.Random.Guid())
            .RuleFor(sak => sak.Organisajonsnummer, f => string.Join("", f.Random.Digits(9)))
            .RuleFor(sak => sak.Status, f => f.PickRandom<SakStatus>())
            .RuleFor(sak => sak.CreatedAt, f => new DateTime(2025, 8, 8, 1, 1, 1, 1))
            .RuleFor(sak => sak.UpdatedAt, f => new DateTime(2025, 9, 9, 1, 1, 1, 1));
}

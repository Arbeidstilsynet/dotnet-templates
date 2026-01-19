using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Db.Model;
using Bogus;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.Test.Fixtures;

internal static class TestData
{
    public static Faker<TilsynssakEntity> CreateSakEntityFaker(int seed = 1337) =>
        new Faker<TilsynssakEntity>()
            .UseSeed(seed)
            .RuleFor(sak => sak.Id, f => f.Random.Guid())
            .RuleFor(sak => sak.Organisajonsnummer, f => string.Join("", f.Random.Digits(9)))
            .RuleFor(sak => sak.Status, f => f.PickRandom<SakStatus>())
            .RuleFor(sak => sak.CreatedAt, f => f.Date.Recent().ToUniversalTime()) // CreatedAt is set to a recent date
            .RuleFor(sak => sak.UpdatedAt, (f, e) => e.CreatedAt + f.Date.Timespan()) // UpdatedAt is after CreatedAt
            .RuleFor(sak => sak.Deadline, (f, e) => e.CreatedAt + TimeSpan.FromDays(30)); // Deadline is 30 days after CreatedAt

    public static Faker<Tilsynssak> CreateSakFaker(int seed = 1337) =>
        new Faker<Tilsynssak>()
            .UseSeed(seed)
            .RuleFor(sak => sak.Id, f => f.Random.Guid())
            .RuleFor(sak => sak.Organisajonsnummer, f => string.Join("", f.Random.Digits(9)))
            .RuleFor(sak => sak.Status, f => f.PickRandom<SakStatus>())
            .RuleFor(sak => sak.CreatedAt, f => f.Date.Recent().ToUniversalTime()) // CreatedAt is set to a recent date
            .RuleFor(sak => sak.LastUpdated, (f, e) => e.CreatedAt + f.Date.Timespan()) // LastUpdated is after CreatedAt
            .RuleFor(sak => sak.Deadline, (f, e) => e.CreatedAt + TimeSpan.FromDays(30)); // Deadline is 30 days after CreatedAt
}

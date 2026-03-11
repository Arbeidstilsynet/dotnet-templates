using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data;
using Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Db.Model;
using Bogus;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Test.Fixtures;

internal static class TestData
{
    public static Faker<SakEntity> CreateSakEntityFaker(int seed = 1337) =>
        new Faker<SakEntity>()
            .UseSeed(seed)
            .RuleFor(sak => sak.Id, f => f.Random.Guid())
            .RuleFor(sak => sak.Organisasjonsnummer, f => string.Join("", f.Random.Digits(9)))
            .RuleFor(sak => sak.Status, f => f.PickRandom<SakStatus>())
            .RuleFor(sak => sak.CreatedAt, f => f.Date.Recent().ToUniversalTime()) // CreatedAt is set to a recent date
            .RuleFor(sak => sak.UpdatedAt, (f, e) => e.CreatedAt + f.Date.Timespan()) // UpdatedAt is after CreatedAt
            .RuleFor(sak => sak.Deadline, (f, e) => e.CreatedAt + TimeSpan.FromDays(30)); // Deadline is 30 days after CreatedAt

    public static Faker<Sak> CreateSakFaker(int seed = 1337) =>
        new Faker<Sak>()
            .UseSeed(seed)
            .RuleFor(sak => sak.Id, f => f.Random.Guid())
            .RuleFor(sak => sak.Organisasjonsnummer, f => string.Join("", f.Random.Digits(9)))
            .RuleFor(sak => sak.Status, f => f.PickRandom<SakStatus>())
            .RuleFor(sak => sak.CreatedAt, f => f.Date.Recent().ToUniversalTime()) // CreatedAt is set to a recent date
            .RuleFor(sak => sak.LastUpdated, (f, e) => e.CreatedAt + f.Date.Timespan()) // LastUpdated is after CreatedAt
            .RuleFor(sak => sak.Deadline, (f, e) => e.CreatedAt + TimeSpan.FromDays(30)); // Deadline is 30 days after CreatedAt
}

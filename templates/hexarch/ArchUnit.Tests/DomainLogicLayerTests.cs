//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.ArchUnit.Tests;

public class DomainLogicLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembliesIncludingDependencies(
            Layers.DomainLogicAssembly,
            Layers.SystemConsoleAssembly
        )
        .Build();

    [Fact]
    public void TypesInDomainLogicLayer_HaveApplicationNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.DomainLogicLayer)
            .Should()
            .ResideInNamespaceMatching(
                $"^({Constants.NameSpacePrefix}\\.Domain\\.Logic|{Constants.NameSpacePrefix}\\.Domain\\.Logic\\..*)$"
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInDomainLogicLayer_ArePublic()
    {
        IArchRule archRule = Types().That().Are(Layers.DomainLogicLayer).Should().BePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInDomainLogicLayer_DoNotDependOnAWS()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.DomainLogicLayer)
            .Should()
            .NotDependOnAny(Types().That().ResideInNamespaceMatching("^Amazon.*$"));

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInDomainLogicLayer_UseCorrectLogger()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.DomainLogicLayer)
            .Should()
            .NotDependOnAny(typeof(Console))
            .Because(
                "We want to use streamlined logging. Try using ILogger<T> via DependencyInjection to log."
            );

        archRule.Check(Architecture);
    }
}

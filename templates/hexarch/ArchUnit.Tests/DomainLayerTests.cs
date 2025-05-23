using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests;

public class DomainLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.DomainAssembly)
        .Build();

    [Fact]
    public void TypesInDomainLayer_HaveDomainNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.DomainLayer)
            .Should()
            .ResideInNamespace(
                $"^({Constants.NameSpacePrefix}\\.Domain\\.Data|{Constants.NameSpacePrefix}\\.Domain\\.Data\\..*)$",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInDomainLayer_ArePublic()
    {
        IArchRule archRule = Types().That().Are(Layers.DomainLayer).Should().BePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInDomainLayer_DoNotDependOnOtherTypesThanDomain()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.DomainLayer)
            .Should()
            .NotDependOnAnyTypesThat()
            .DoNotResideInNamespace(
                $"(^Coverlet.Core.Instrumentation.*$|^System.*$|^{Constants.NameSpacePrefix}\\.Domain\\.Data.*$)",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInDomainLayer_ShouldNotUseLoggerAtAll()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.DomainLayer)
            .Should()
            .NotDependOnAny(typeof(System.Console))
            .Because("This layer describes our core model and should not contain any logic.");

        archRule.Check(Architecture);
    }
}

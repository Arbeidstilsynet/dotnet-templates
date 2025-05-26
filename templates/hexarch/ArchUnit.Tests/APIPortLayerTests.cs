using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests;

public class APIPortLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.APIPortAssembly)
        .Build();

    [Fact]
    public void TypesInAPIPortLayer_HaveAPIPortsNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.APIPortLayer)
            .Should()
            .ResideInNamespace(
                $"^({Constants.NameSpacePrefix}\\.API\\.Ports|{Constants.NameSpacePrefix}\\.API\\.Ports\\..*)$",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInAPIPortLayer_ArePublic()
    {
        IArchRule archRule = Types().That().Are(Layers.APIPortLayer).Should().BePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInAPIPortLayer_DoNotDependOnOtherTypesThanAPIPorts()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.APIPortLayer)
            .Should()
            .NotDependOnAnyTypesThat()
            .DoNotResideInNamespace(
                $"(^Coverlet.Core.Instrumentation.*$|^System.*$|^{Constants.NameSpacePrefix}\\.API\\.Ports.*$|^{Constants.NameSpacePrefix}\\.Domain\\.Data.*$)",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInAPIPortLayer_ShouldNotUseLoggerAtAll()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.APIPortLayer)
            .Should()
            .NotDependOnAny(typeof(System.Console))
            .Because(
                "This layer should only define how the other layers can be accessed. Therefor it should only contain interfaces and DTOs."
            );

        archRule.Check(Architecture);
    }
}

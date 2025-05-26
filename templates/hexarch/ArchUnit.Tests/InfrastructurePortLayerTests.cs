using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests;

public class InfrastructurePortLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.InfrastructurePortAssembly)
        .Build();

    [Fact]
    public void TypesInInfrastructurePortLayer_HaveInfrastructurePortsNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.InfrastructurePortLayer)
            .Should()
            .ResideInNamespace(
                $"^({Constants.NameSpacePrefix}\\.Infrastructure\\.Ports|{Constants.NameSpacePrefix}\\.Infrastructure\\.Ports\\..*)$",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInInfrastructurePortLayer_ArePublic()
    {
        IArchRule archRule = Types().That().Are(Layers.InfrastructurePortLayer).Should().BePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInInfrastructurePortLayer_DoNotDependOnOtherTypesThanInfrastructurePortsAndDomain()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.InfrastructurePortLayer)
            .Should()
            .NotDependOnAnyTypesThat()
            .DoNotResideInNamespace(
                $"(^Coverlet.Core.Instrumentation.*$|^System.*$|^{Constants.NameSpacePrefix}\\.Infrastructure\\.Ports.*$|^{Constants.NameSpacePrefix}\\.Domain\\.Data.*$)",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInInfrastructurePortLayer_ShouldNotUseLoggerAtAll()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.InfrastructurePortLayer)
            .Should()
            .NotDependOnAny(typeof(System.Console))
            .Because(
                "This layer should only define how the other layers can be accessed. Therefor it should only contain interfaces and DTOs."
            );

        archRule.Check(Architecture);
    }
}

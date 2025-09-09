using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Microsoft.Extensions.Logging;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests;

public class ApiPortLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.ApiPortAssembly, Layers.SystemConsoleAssembly)
        .Build();

    [Fact]
    public void TypesInAPIPortLayer_HaveAPIPortsNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.ApiPortLayer)
            .Should()
            .ResideInNamespaceMatching(
                $"^({Constants.NameSpacePrefix}\\.API\\.Ports|{Constants.NameSpacePrefix}\\.API\\.Ports\\..*)$"
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInAPIPortLayer_ArePublic()
    {
        IArchRule archRule = Types().That().Are(Layers.ApiPortLayer).Should().BePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInAPIPortLayer_DoNotDependOnOtherTypesThanAPIPorts()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.ApiPortLayer)
            .Should()
            .NotDependOnAny(
                Types()
                    .That()
                    .DoNotResideInNamespaceMatching(
                        $"^(System.*|{Constants.NameSpacePrefix}\\.API\\.Ports|{Constants.NameSpacePrefix}\\.API\\.Ports\\..*|{Constants.NameSpacePrefix}\\.Domain\\.Data.*)$"
                    )
            );

        archRule.Check(Architecture);
    }
}

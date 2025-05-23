using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace SamplePackage.ArchUnit.Tests;

public class SamplePackagePortLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.SamplePackagePortAssembly)
        .Build();

    [Fact]
    public void TypesInSamplePackagePortLayerLayer_HaveCorrectNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackagePortLayer)
            .Should()
            .ResideInNamespace(
                $"^({Constants.NameSpacePrefix}\\.Ports|{Constants.NameSpacePrefix}\\.Ports\\..*)$",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackagePortLayerLayer_ArePublic()
    {
        IArchRule archRule = Types().That().Are(Layers.SamplePackagePortLayer).Should().BePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackagePortLayerLayer_DoNotDependOnOtherTypesThanDefaultTypes()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackagePortLayer)
            .Should()
            .NotDependOnAnyTypesThat()
            .DoNotResideInNamespace(
                $"(^Coverlet.Core.Instrumentation.*$|^System.*$|^{Constants.NameSpacePrefix}.*$)",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackagePortLayerLayer_ShouldNotUseLoggerAtAll()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackagePortLayer)
            .Should()
            .NotDependOnAny(typeof(System.Console))
            .Because("This layer describes our core model and should not contain any logic.");

        archRule.Check(Architecture);
    }
}

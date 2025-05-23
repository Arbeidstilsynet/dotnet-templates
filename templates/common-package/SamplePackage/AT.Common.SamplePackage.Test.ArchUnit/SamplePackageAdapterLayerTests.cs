using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace SamplePackage.ArchUnit.Tests;

public class SamplePackageAdapterLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.SamplePackageAdapterAssembly)
        .Build();

    [Fact]
    public void TypesInSamplePackageAdapterLayer_HaveCorrectNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageAdapterLayer)
            .Should()
            .ResideInNamespace(
                $"^({Constants.NameSpacePrefix}\\.Adapters|{Constants.NameSpacePrefix}\\.Adapters\\..*)$",
                true
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackageAdapterLayer_AreInternal()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageAdapterLayer)
            .And()
            .DoNotResideInNamespace(
                $"^({Constants.NameSpacePrefix}\\.Adapters\\.DependencyInjection|{Constants.NameSpacePrefix}\\.Adapters\\.DependencyInjection\\..*)$",
                true
            )
            .Should()
            .NotBePublic();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackageAdapterLayer_DoNotDependOnAWS()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageAdapterLayer)
            .Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("^Amazon.*$", true);

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackageAdapterLayer_UseCorrectLogger()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageAdapterLayer)
            .Should()
            .NotDependOnAny(typeof(System.Console))
            .Because(
                "We want to use streamlined logging. Try using ILogger<T> via DependencyInjection to log."
            );

        archRule.Check(Architecture);
    }
}

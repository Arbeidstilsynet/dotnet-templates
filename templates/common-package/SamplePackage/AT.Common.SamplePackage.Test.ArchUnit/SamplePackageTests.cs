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
        .LoadAssemblies(Layers.SamplePackageAssembly)
        .Build();

    [Fact]
    public void TypesInSamplePackage_HaveCorrectNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageLayer)
            .Should()
            .ResideInNamespace(Constants.RootNamespace, true);

        archRule.Check(Architecture);
    }

    [Fact]
    public void InterfaceImplementationsInSamplePackage_AreNotPublic()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageLayer)
            .And()
            .AreAssignableTo(Layers.SamplePackagePublicInterfaces)
            .And()
            .AreNot(Layers.SamplePackagePublicInterfaces)
            .Should()
            .NotBePublic()
            .Because("Implementations should not be public.");

        archRule.Check(Architecture);
    }

    [Fact]
    public void PublicClasses_MustResideInExtensionsOrDependencyInjectionOrModelNamespaces()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageLayer)
            .And()
            .AreNot(Layers.SamplePackagePublicInterfaces)
            .And()
            .DoNotResideInNamespace(Constants.ExtensionsNamespace, true)
            .And()
            .DoNotResideInNamespace(Constants.DependencyInjectionNamespace, true)
            .And()
            .DoNotResideInNamespace(Constants.ModelNamespace, true)
            .Should()
            .NotBePublic()
            .Because(
                "Public classes should reside in either *.Extensions, *.DependencyInjection, or *.Model namespaces."
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackageAdapterLayer_DoNotDependOnAWS()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageLayer)
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
            .Are(Layers.SamplePackageLayer)
            .Should()
            .NotDependOnAny(typeof(System.Console))
            .Because(
                "We want to use streamlined logging. Try using ILogger<T> via DependencyInjection to log."
            );

        archRule.Check(Architecture);
    }
}

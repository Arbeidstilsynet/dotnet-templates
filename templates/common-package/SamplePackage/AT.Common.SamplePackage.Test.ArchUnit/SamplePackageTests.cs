//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Arbeidstilsynet.Common.SamplePackage.Test.ArchUnit;

public class SamplePackageAdapterLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.SamplePackageAssembly, Layers.SystemConsoleAssembly)
        .Build();

    [Fact]
    public void TypesInSamplePackage_HaveCorrectNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageLayer)
            .Should()
            .ResideInNamespaceMatching(Constants.RootNamespace)
            .WithoutRequiringPositiveResults();

        archRule.Check(Architecture);
    }

    [Fact]
    public void InterfaceImplementationsInSamplePackage_AreNotPublic()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.InterfaceImplementations)
            .Should()
            .NotBePublic()
            .WithoutRequiringPositiveResults();

        archRule.Check(Architecture);
    }

    [Fact]
    public void PublicNonAbstractClasses_MustResideInExtensionsOrDependencyInjectionOrModelNamespaces()
    {
        IArchRule archRule = Types()
            .That()
            .AreNot(Layers.PublicInterfaces)
            .And()
            .AreNot(Layers.PublicAbstractClasses)
            .And()
            .Are(Layers.TypesInInternalNamespaces)
            .Should()
            .NotBePublic()
            .Because(
                "public types should either be an abstract class, an interface OR reside in a namespace containing \"Extensions\", \"DependencyInjection\" or \"Model\"."
            )
            .WithoutRequiringPositiveResults();

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInSamplePackageAdapterLayer_DoNotDependOnAWS()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.SamplePackageLayer)
            .Should()
            .NotDependOnAny(Types().That().ResideInNamespaceMatching("^Amazon.*$"));

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
                "we want to use streamlined logging. Try using ILogger<T> via DependencyInjection to log."
            );

        archRule.Check(Architecture);
    }
}

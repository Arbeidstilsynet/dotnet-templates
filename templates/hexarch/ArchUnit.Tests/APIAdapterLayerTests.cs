using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests;

public class ApiAdapterLayerTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(Layers.ApiAdapterAssembly, Layers.SystemConsoleAssembly)
        .Build();

    [Fact]
    public void TypesInAPIAdapterLayer_HaveAPIAdapterNamespace()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.ApiAdapterLayer)
            .And()
            // top level class cannot have any namespace
            .DoNotHaveFullName("Program")
            .Should()
            .ResideInNamespaceMatching(
                $"^({Constants.NameSpacePrefix}\\.API\\.Adapters|{Constants.NameSpacePrefix}\\.API\\.Adapters\\..*)$"
            );

        archRule.Check(Architecture);
    }

    [Fact]
    public void TypesInAPIAdapterLayer_UseCorrectLogger()
    {
        IArchRule archRule = Types()
            .That()
            .Are(Layers.ApiAdapterLayer)
            .Should()
            .NotDependOnAny(typeof(Console))
            .Because(
                "We want to use streamlined logging. Try using ILogger<T> via DependencyInjection to log."
            );
        archRule.Check(Architecture);
    }
}

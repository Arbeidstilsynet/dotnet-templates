//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.ArchUnit.Tests;

public class ArchUnitTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            Layers.DomainLogicAssembly,
            Layers.ApiAdapterAssembly,
            Layers.InfrastructureAdapterAssembly,
            Layers.DomainAssembly
        )
        .Build();

}

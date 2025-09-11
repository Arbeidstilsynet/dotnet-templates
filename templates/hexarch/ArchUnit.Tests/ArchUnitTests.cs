using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
//add a using directive to ArchUnitNET.Fluent.ArchRuleDefinition to easily define ArchRules
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests;

public class ArchUnitTests
{
    static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            Layers.DomainLogicAssembly,
            Layers.ApiPortAssembly,
            Layers.ApiAdapterAssembly,
            Layers.InfrastructureAdapterAssembly,
            Layers.InfrastructurePortAssembly,
            Layers.DomainAssembly
        )
        .Build();

    [Fact]
    public void DomainShouldNotDependOnAnyOtherLayer()
    {
        var domainshouldOnlyDependOnItself = Types()
            .That()
            .Are(Layers.DomainLayer)
            .Should()
            .OnlyDependOn(Layers.DomainLayer)
            .Because(
                $"{Layers.DomainLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.DomainLayer.Description}?"
            );

        domainshouldOnlyDependOnItself.Check(Architecture);
    }

    [Fact]
    public void InfrastructureAdapterLayerShouldOnlyAccessInfrastructurePortLayer()
    {
        var adapterLayerShouldNotAccessApplicationLayer = Types()
            .That()
            .Are(Layers.InfrastructureAdapterLayer)
            .Should()
            .NotDependOnAny(Layers.DomainLogicLayer)
            .Because(
                $"{Layers.InfrastructureAdapterLayer.Description} should only access {Layers.InfrastructurePortLayer.Description}. Did you add new project references in {Layers.InfrastructureAdapterLayer.Description}?"
            );

        var adapterLayerShouldNotAccessApiPortLayer = Types()
            .That()
            .Are(Layers.InfrastructureAdapterLayer)
            .Should()
            .NotDependOnAny(Layers.ApiPortLayer)
            .Because(
                $"{Layers.InfrastructureAdapterLayer.Description} should only access {Layers.InfrastructurePortLayer.Description}. Did you add new project references in {Layers.InfrastructureAdapterLayer.Description}?"
            );

        var combinedArchRule = adapterLayerShouldNotAccessApplicationLayer.And(
            adapterLayerShouldNotAccessApiPortLayer
        );

        combinedArchRule.Check(Architecture);
    }

    [Fact]
    public void InfrastructurePortLayerShouldNotAccessAnyOtherLayersAtAll()
    {
        var portLayerShouldNotAccessInfrastructureAdapterLayer = Types()
            .That()
            .Are(Layers.InfrastructurePortLayer)
            .Should()
            .NotDependOnAny(Layers.InfrastructureAdapterLayer)
            .Because(
                $"{Layers.InfrastructurePortLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.InfrastructurePortLayer.Description}?"
            );

        var portLayerShouldNotAccessApplicationLayer = Types()
            .That()
            .Are(Layers.InfrastructurePortLayer)
            .Should()
            .NotDependOnAny(Layers.DomainLogicLayer)
            .Because(
                $"{Layers.InfrastructurePortLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.InfrastructurePortLayer.Description}?"
            );

        var portLayerShouldNotAccessApiPortLayer = Types()
            .That()
            .Are(Layers.InfrastructurePortLayer)
            .Should()
            .NotDependOnAny(Layers.ApiPortLayer)
            .Because(
                $"{Layers.InfrastructurePortLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.InfrastructurePortLayer.Description}?"
            );

        var combinedArchRule = portLayerShouldNotAccessInfrastructureAdapterLayer
            .And(portLayerShouldNotAccessApplicationLayer)
            .And(portLayerShouldNotAccessApiPortLayer);

        combinedArchRule.Check(Architecture);
    }

    [Fact]
    public void ApiPortLayerShouldNotAccessAnyOtherLayersAtAll()
    {
        var portLayerShouldNotAccessInfrastructureAdapterLayer = Types()
            .That()
            .Are(Layers.ApiPortLayer)
            .Should()
            .NotDependOnAny(Layers.InfrastructureAdapterLayer)
            .Because(
                $"{Layers.ApiPortLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.ApiPortLayer.Description}?"
            );

        var portLayerShouldNotAccessApplicationLayer = Types()
            .That()
            .Are(Layers.ApiPortLayer)
            .Should()
            .NotDependOnAny(Layers.DomainLogicLayer)
            .Because(
                $"{Layers.ApiPortLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.ApiPortLayer.Description}?"
            );

        var portLayerShouldNotAccessInfrastructurePortLayer = Types()
            .That()
            .Are(Layers.ApiPortLayer)
            .Should()
            .NotDependOnAny(Layers.InfrastructurePortLayer)
            .Because(
                $"{Layers.ApiPortLayer.Description} should not access any other layers at all. Did you add new project references in {Layers.ApiPortLayer.Description}?"
            );

        var combinedArchRule = portLayerShouldNotAccessInfrastructureAdapterLayer
            .And(portLayerShouldNotAccessApplicationLayer)
            .And(portLayerShouldNotAccessInfrastructurePortLayer);

        combinedArchRule.Check(Architecture);
    }

    [Fact]
    public void APIAdapterLayerShouldOnlyAccessPortLayers()
    {
        var adapterLayerShouldNotAccessApplicationLayer = Types()
            .That()
            .Are(Layers.ApiAdapterLayer)
            .Should()
            .NotDependOnAny(Layers.DomainLogicLayer)
            .Because(
                $"{Layers.InfrastructureAdapterLayer.Description} should only have direct code references to {Layers.InfrastructurePortLayer.Description} or {Layers.ApiPortLayer.Description}."
            );

        var adapterLayerShouldNotAccessInfrastructureAdapterLayer = Types()
            .That()
            .Are(Layers.ApiAdapterLayer)
            .Should()
            .NotDependOnAny(Layers.InfrastructureAdapterLayer)
            .Because(
                $"{Layers.InfrastructureAdapterLayer.Description} should only have direct code references to {Layers.InfrastructurePortLayer.Description} or {Layers.ApiPortLayer.Description}."
            );

        var combinedArchRule = adapterLayerShouldNotAccessApplicationLayer.And(
            adapterLayerShouldNotAccessInfrastructureAdapterLayer
        );

        combinedArchRule.Check(Architecture);
    }

    [Fact]
    public void ApplicationLayerShouldOnlyAccessDomainAndPortLayers()
    {
        var applicationLayerShouldNotAccessInfrastructureAdapterLayer = Types()
            .That()
            .Are(Layers.DomainLogicLayer)
            .Should()
            .NotDependOnAny(Layers.InfrastructureAdapterLayer)
            .Because(
                $"{Layers.DomainLogicLayer.Description} should only access {Layers.DomainLayer.Description}, {Layers.ApiPortLayer.Description} or {Layers.InfrastructurePortLayer.Description}. Did you add new project references in {Layers.DomainLogicLayer.Description}?"
            );

        var combinedArchRule = applicationLayerShouldNotAccessInfrastructureAdapterLayer;

        combinedArchRule.Check(Architecture);
    }
}

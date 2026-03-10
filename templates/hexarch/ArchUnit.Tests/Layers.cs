using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.ArchUnit.Tests
{
    internal static class Constants
    {
        internal static string NameSpacePrefix =
            $"Arbeidstilsynet\\.{Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.IAssemblyInfo.AppName}";

        internal static string CoverageCollectorNamespace =
            "Microsoft.CodeCoverage.Instrumentation.Static.Tracker";
    }

    internal static class Layers
    {
        internal static readonly System.Reflection.Assembly DomainLogicAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly ApiPortAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.App.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly InfrastructureAdapterAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly InfrastructurePortAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Ports.Infrastructure.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly DomainAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly ApiAdapterAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App.IAssemblyInfo).Assembly;

        internal static readonly System.Reflection.Assembly SystemConsoleAssembly =
            typeof(Console).Assembly;

        internal static readonly IObjectProvider<IType> DomainLogicLayer = Types()
            .That()
            .ResideInAssembly(DomainLogicAssembly)
            .And()
            .DoNotResideInNamespaceMatching(
                $"^({Constants.CoverageCollectorNamespace}|{Constants.NameSpacePrefix}\\.Domain\\.Logic\\.DependencyInjection|{Constants.NameSpacePrefix}\\.Domain\\.Logic\\.DependencyInjection\\..*)$"
            )
            .As("Application Service Layer");
        internal static readonly IObjectProvider<IType> InfrastructureAdapterLayer = Types()
            .That()
            .ResideInAssembly(InfrastructureAdapterAssembly)
            .And()
            .DoNotResideInNamespaceMatching(
                $"^({Constants.CoverageCollectorNamespace}|{Constants.NameSpacePrefix}\\.Infrastructure\\.DependencyInjection|{Constants.NameSpacePrefix}\\.Infrastructure\\.DependencyInjection\\..*)$"
            )
            .As("Infrastructure Adapter Layer");
        internal static readonly IObjectProvider<IType> InfrastructurePortLayer = Types()
            .That()
            .ResideInAssembly(InfrastructurePortAssembly)
            .And()
            .DoNotResideInNamespace(Constants.CoverageCollectorNamespace)
            .As("Infrastructure Port Layer");
        internal static readonly IObjectProvider<IType> ApiPortLayer = Types()
            .That()
            .ResideInAssembly(ApiPortAssembly)
            .And()
            .DoNotResideInNamespace(Constants.CoverageCollectorNamespace)
            .As("API Port Layer");
        internal static readonly IObjectProvider<IType> DomainLayer = Types()
            .That()
            .ResideInAssembly(DomainAssembly)
            .And()
            .DoNotResideInNamespace(Constants.CoverageCollectorNamespace)
            .As("Domain Layer");
        internal static readonly IObjectProvider<IType> ApiAdapterLayer = Types()
            .That()
            .ResideInAssembly(ApiAdapterAssembly)
            .And()
            .DoNotResideInNamespace(Constants.CoverageCollectorNamespace)
            .As("API Adapter Layer");
    }
}

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnit.Tests
{
    internal static class Constants
    {
        internal static string NameSpacePrefix =
            $"Arbeidstilsynet\\.{Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.IAssemblyInfo.AppName}";
    }

    internal static class Layers
    {
        internal static readonly System.Reflection.Assembly DomainLogicAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly APIPortAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Ports.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly InfrastructureAdapterAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly InfrastructurePortAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Ports.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly DomainAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Data.IAssemblyInfo).Assembly;
        internal static readonly System.Reflection.Assembly APIAdapterAssembly =
            typeof(Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters.IAssemblyInfo).Assembly;

        internal static readonly System.Reflection.Assembly SystemConsoleAssembly =
            typeof(System.Console).Assembly;

        internal static readonly IObjectProvider<IType> DomainLogicLayer = Types()
            .That()
            .ResideInAssembly(DomainLogicAssembly)
            .And()
            .DoNotResideInNamespaceMatching("Coverlet.Core.Instrumentation.Tracker")
            .And()
            .DoNotResideInNamespaceMatching(
                $"^({Constants.NameSpacePrefix}\\.Domain\\.Logic\\.DependencyInjection|{Constants.NameSpacePrefix}\\.Domain\\.Logic\\.DependencyInjection\\..*)$"
            )
            .As("Application Service Layer");
        internal static readonly IObjectProvider<IType> InfrastructureAdapterLayer = Types()
            .That()
            .ResideInAssembly(InfrastructureAdapterAssembly)
            .And()
            .DoNotResideInNamespaceMatching(
                $"^({Constants.NameSpacePrefix}\\.Infrastructure\\.Adapters\\.DependencyInjection|{Constants.NameSpacePrefix}\\.Infrastructure\\.Adapters\\.DependencyInjection\\..*)$"
            )
            .And()
            .DoNotResideInNamespaceMatching("Coverlet.Core.Instrumentation.Tracker")
            .As("Infrastructure Adapter Layer");
        internal static readonly IObjectProvider<IType> InfrastructurePortLayer = Types()
            .That()
            .ResideInAssembly(InfrastructurePortAssembly)
            .And()
            .DoNotResideInNamespaceMatching("Coverlet.Core.Instrumentation.Tracker")
            .As("Infrastructure Port Layer");
        internal static readonly IObjectProvider<IType> APIPortLayer = Types()
            .That()
            .ResideInAssembly(APIPortAssembly)
            .And()
            .DoNotResideInNamespaceMatching("Coverlet.Core.Instrumentation.Tracker")
            .As("API Port Layer");
        internal static readonly IObjectProvider<IType> DomainLayer = Types()
            .That()
            .ResideInAssembly(DomainAssembly)
            .And()
            .DoNotResideInNamespaceMatching("Coverlet.Core.Instrumentation.Tracker")
            .As("Domain Layer");
        internal static readonly IObjectProvider<IType> APIAdapterLayer = Types()
            .That()
            .ResideInAssembly(APIAdapterAssembly)
            .And()
            .DoNotResideInNamespaceMatching("Coverlet.Core.Instrumentation.Tracker")
            .As("API Adapter Layer");
    }
}

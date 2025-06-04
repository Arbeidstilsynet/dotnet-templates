using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace SamplePackage.ArchUnit.Tests
{
    internal static class Constants
    {
        internal static string NameSpacePrefix = $"Arbeidstilsynet\\.Common\\.SamplePackage";
    }

    internal static class Layers
    {
        internal static readonly System.Reflection.Assembly SamplePackagePortAssembly =
            typeof(Arbeidstilsynet.Common.SamplePackage.Ports.IAssemblyInfo).Assembly;

        internal static readonly System.Reflection.Assembly SamplePackageAdapterAssembly =
            typeof(Arbeidstilsynet.Common.SamplePackage.Adapters.IAssemblyInfo).Assembly;

        internal static readonly IObjectProvider<IType> SamplePackagePortLayer = Types()
            .That()
            .ResideInAssembly(SamplePackagePortAssembly)
            .And()
            .DoNotResideInNamespace("Coverlet.Core.Instrumentation.Tracker")
            .As("SamplePackage Port Layer");

        internal static readonly IObjectProvider<IType> SamplePackageAdapterLayer = Types()
            .That()
            .ResideInAssembly(SamplePackageAdapterAssembly)
            .And()
            .DoNotResideInNamespace("Coverlet.Core.Instrumentation.Tracker")
            .As("SamplePackage Adapter Layer");
    }
}

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace SampleExtension.ArchUnit.Tests
{
    internal static class Constants
    {
        internal static string NameSpacePrefix = $"Arbeidstilsynet\\.Common\\.SampleExtension";
    }

    internal static class Layers
    {
        internal static readonly System.Reflection.Assembly SampleExtensionAssembly =
            typeof(Arbeidstilsynet.Common.SampleExtension.Extensions.IAssemblyInfo).Assembly;

        internal static readonly IObjectProvider<IType> SampleExtensionLayer = Types()
            .That()
            .ResideInAssembly(SampleExtensionAssembly)
            .And()
            .DoNotResideInNamespace("Coverlet.Core.Instrumentation.Tracker")
            .As("SampleExtension Port Layer");
    }
}

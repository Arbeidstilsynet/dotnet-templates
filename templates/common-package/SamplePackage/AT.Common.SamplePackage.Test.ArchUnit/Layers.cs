using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace SamplePackage.ArchUnit.Tests
{
    internal static class Constants
    {
        internal static string NameSpacePrefix = @"Arbeidstilsynet\.Common\.SamplePackage";
        internal static string RootNamespace = $"^({NameSpacePrefix}|{NameSpacePrefix}\\..*)$";
        internal static string ExtensionsNamespace = CreateNamespaceRegex("Extensions");
        internal static string DependencyInjectionNamespace = CreateNamespaceRegex(
            "DependencyInjection"
        );
        internal static string ModelNamespace = CreateNamespaceRegex("Model");

        private static string CreateNamespaceRegex(string postfix)
        {
            return $@"^({NameSpacePrefix}\.*\.{postfix}|{NameSpacePrefix}\.{postfix})$";
        }
    }

    internal static class Layers
    {
        internal static readonly System.Reflection.Assembly SamplePackageAssembly =
            typeof(Arbeidstilsynet.Common.SamplePackage.IAssemblyInfo).Assembly;

        internal static readonly IObjectProvider<IType> SamplePackageLayer = Types()
            .That()
            .ResideInAssembly(SamplePackageAssembly)
            .And()
            .DoNotResideInNamespace("Coverlet.Core.Instrumentation.Tracker")
            .As("SamplePackage Layer");

        internal static readonly IObjectProvider<IType> SamplePackagePublicInterfaces = Interfaces()
            .That()
            .ResideInAssembly(SamplePackageAssembly)
            .And()
            .ArePublic()
            .As("SamplePackage Public Interfaces");
    }
}

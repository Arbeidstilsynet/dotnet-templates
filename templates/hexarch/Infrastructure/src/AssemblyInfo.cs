using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchUnit.Tests")]
[assembly: InternalsVisibleTo("Infrastructure.Test")]
[assembly: InternalsVisibleTo("App.Test")]

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure;

interface IAssemblyInfo { }

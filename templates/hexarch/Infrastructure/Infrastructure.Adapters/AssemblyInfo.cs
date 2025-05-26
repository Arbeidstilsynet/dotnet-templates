using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchUnit.Tests")]
[assembly: InternalsVisibleTo("Infrastructure.Adapters.Test")]
[assembly: InternalsVisibleTo("API.Adapters.Test")]

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure.Adapters;

interface IAssemblyInfo { }

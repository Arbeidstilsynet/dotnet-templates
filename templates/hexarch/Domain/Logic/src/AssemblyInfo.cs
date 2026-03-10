using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchUnit.Tests")]
[assembly: InternalsVisibleTo("Domain.Logic.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // Required for NSubstitute to make mocks of internal classes

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

interface IAssemblyInfo { }

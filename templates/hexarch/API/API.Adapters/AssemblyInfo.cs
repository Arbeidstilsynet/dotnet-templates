using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("API.Adapters.Test")]

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;

public interface IAssemblyInfo
{
    public const string AppName = "HexagonalArchitectureTemplateDocker";
}

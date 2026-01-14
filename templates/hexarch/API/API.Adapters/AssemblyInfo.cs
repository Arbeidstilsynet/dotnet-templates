using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("API.Adapters.Test")]

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.API.Adapters;

/// <summary>
/// Marker interface for assembly information.
/// </summary>
public interface IAssemblyInfo
{
    /// <summary>
    /// The name of the application.
    /// </summary>
    public const string AppName = "HexagonalArchitectureTemplateDocker";
}

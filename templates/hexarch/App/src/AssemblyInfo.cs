using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("App.Test")]

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App;

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

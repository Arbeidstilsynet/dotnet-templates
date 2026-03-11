using System.Diagnostics;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.App;

internal static class Tracer
{
    public static readonly ActivitySource Source = new("App");
}

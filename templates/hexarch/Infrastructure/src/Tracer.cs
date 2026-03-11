using System.Diagnostics;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Infrastructure;

internal static class Tracer
{
    public static readonly ActivitySource Source = new("Infrastructure");
}

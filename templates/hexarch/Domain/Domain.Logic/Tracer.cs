using System.Diagnostics;

namespace Arbeidstilsynet.HexagonalArchitectureTemplateDocker.Domain.Logic;

internal static class Tracer
{
    public static readonly ActivitySource Source = new("Domain.Logic");
}

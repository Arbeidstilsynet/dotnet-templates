namespace Arbeidstilsynet.Common.SamplePackage.Model;

/// <summary>
/// DTOs should be under the *.Model namespace.
/// </summary>
public record SamplePackageDto
{
    /// <summary>
    /// Required summary for public property.
    /// </summary>
    public required string Foo { get; init; }
}

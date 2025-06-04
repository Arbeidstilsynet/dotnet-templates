using Arbeidstilsynet.Common.SamplePackage.Model;

namespace Arbeidstilsynet.Common.SamplePackage;

/// <summary>
/// Interface which can be dependency injected to use methods of SamplePackage
/// </summary>
public interface ISamplePackage
{
    Task<SamplePackageDto> Get();
}

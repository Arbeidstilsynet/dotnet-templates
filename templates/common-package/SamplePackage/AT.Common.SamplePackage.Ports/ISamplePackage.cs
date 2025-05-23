using Arbeidstilsynet.Common.SamplePackage.Ports.Model;

namespace Arbeidstilsynet.Common.SamplePackage.Ports;

/// <summary>
/// Interface which can be dependency injected to use mehtods of SamplePackage
/// </summary>
public interface ISamplePackage
{
    Task<SamplePackageDto> Get();
}

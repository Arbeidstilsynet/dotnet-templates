using Arbeidstilsynet.Common.SamplePackage.Model;

namespace Arbeidstilsynet.Common.SamplePackage;

/// <summary>
/// Interface which can be dependency injected to use methods of SamplePackage
/// </summary>
public interface ISamplePackage
{
    /// <summary>
    /// Required XML summary of the Get method
    /// </summary>
    /// <returns></returns>
    Task<SamplePackageDto> Get();
}

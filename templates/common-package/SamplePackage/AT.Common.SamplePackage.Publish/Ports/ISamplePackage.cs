using Arbeidstilsynet.Common.SamplePackage.DependencyInjection;
using Arbeidstilsynet.Common.SamplePackage.Model;

namespace Arbeidstilsynet.Common.SamplePackage.Ports;

/// <summary>
/// Use the <see cref="DependencyInjectionExtensions.AddSamplePackage"/> method to inject an implementation of this interface.
/// </summary>
public interface ISamplePackage
{
    /// <summary>
    /// Required XML summary of the Get method
    /// </summary>
    /// <returns></returns>
    public Task<SamplePackageDto> Get();
}

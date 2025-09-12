using Arbeidstilsynet.Common.SamplePackage.Model;
using Arbeidstilsynet.Common.SamplePackage.Ports;

namespace Arbeidstilsynet.Common.SamplePackage.Implementation;

/// <summary>
/// Implementations should not be public.
/// </summary>
internal class SamplePackageImplementation : ISamplePackage
{
    public Task<SamplePackageDto> Get()
    {
        return Task.FromResult(new SamplePackageDto { Foo = "Bar" });
    }
}

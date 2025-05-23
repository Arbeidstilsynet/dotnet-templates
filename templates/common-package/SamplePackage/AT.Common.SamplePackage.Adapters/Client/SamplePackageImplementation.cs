using Arbeidstilsynet.Common.SamplePackage.Ports;
using Arbeidstilsynet.Common.SamplePackage.Ports.Model;

namespace Arbeidstilsynet.Common.SamplePackage.Adapters;

internal class SamplePackageImplementation : ISamplePackage
{
    public Task<SamplePackageDto> Get()
    {
        return Task.FromResult(new SamplePackageDto { Foo = "Bar" });
    }
}

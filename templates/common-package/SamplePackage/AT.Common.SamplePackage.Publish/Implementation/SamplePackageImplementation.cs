using Arbeidstilsynet.Common.SamplePackage.Model;

namespace Arbeidstilsynet.Common.SamplePackage.Implementation;

internal class SamplePackageImplementation : ISamplePackage
{
    public Task<SamplePackageDto> Get()
    {
        return Task.FromResult(new SamplePackageDto { Foo = "Bar" });
    }
}

using Arbeidstilsynet.Common.SamplePackage.Adapters;
using Shouldly;
using Xunit;

namespace Arbeidstilsynet.Common.SamplePackage.Test;

public class SamplePackageTests
{
    private readonly SamplePackageImplementation _sut;

    public SamplePackageTests()
    {
        _sut = new SamplePackageImplementation();
    }

    [Fact]
    public async Task Get_WhenCalled_ReturnsBar()
    {
        //arrange

        //act
        var result = await _sut.Get();
        //assert
        result.Foo.ShouldBe("Bar 2");
    }
}

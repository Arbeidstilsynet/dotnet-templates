using Arbeidstilsynet.Common.SamplePackage.Implementation;
using Shouldly;
using Xunit;

namespace Arbeidstilsynet.Common.SamplePackage.Test.Unit;

public class SamplePackageTests
{
    private readonly SamplePackageImplementation _sut = new();

    [Fact]
    public async Task Get_WhenCalled_ReturnsBar()
    {
        //arrange

        //act
        var result = await _sut.Get();
        //assert
        result.Foo.ShouldBe("Bar");
    }
}

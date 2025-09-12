using Arbeidstilsynet.Common.SamplePackage.Extensions;
using Arbeidstilsynet.Common.SamplePackage.Model;
using Shouldly;
using Xunit;

namespace Arbeidstilsynet.Common.SamplePackage.Test.Unit;

public class SamplePackageExtensionsTests
{
    [Fact]
    public void ToUpper_WhenCalled_ReturnsUppercaseFoo()
    {
        // Arrange
        var model = new SamplePackageDto() { Foo = "Bar" };

        // Act
        var result = model.ToUpper();

        // Assert
        result.Foo.ShouldBe("BAR");
    }
}

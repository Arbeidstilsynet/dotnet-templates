using Arbeidstilsynet.Common.SamplePackage.Extensions;
using Arbeidstilsynet.Common.SamplePackage.Model;
using Shouldly;
using Xunit;

namespace Arbeidstilsynet.Common.SamplePackage.Test;

public class SamplePackageExtensionsTests
{
    [Fact]
    public void SamplePackageExtensions_ShouldHaveCorrectNamespace()
    {
        // Arrange
        var model = new SamplePackageDto() { Foo = "Bar" };

        // Act
        var result = model.ToUpper();

        // Assert
        result.Foo.ShouldBe("BAR");
    }
}

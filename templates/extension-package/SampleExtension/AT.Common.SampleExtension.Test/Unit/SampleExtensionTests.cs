using Arbeidstilsynet.Common.SampleExtension.Extensions;
using Shouldly;
using Xunit;

namespace Arbeidstilsynet.Common.SampleExtension.Test;

public class SampleExtensionTests
{
    public SampleExtensionTests() { }

    [Fact]
    public void WordCountExtension_WhenCalledWithOneWord_ReturnsOne()
    {
        //arrange

        //act
        var result = "test".WordCount();
        //assert
        result.ShouldBe(1);
    }
}

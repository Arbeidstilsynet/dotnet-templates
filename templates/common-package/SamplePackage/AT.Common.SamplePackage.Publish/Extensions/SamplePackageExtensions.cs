using Arbeidstilsynet.Common.SamplePackage.Model;

namespace Arbeidstilsynet.Common.SamplePackage.Extensions;

/// <summary>
/// Extensions for SamplePackage
/// </summary>
public static class SamplePackageExtensions
{
    /// <summary>
    /// Dummy extension method for demo
    /// </summary>
    /// <param name="dto">The dto to extend</param>
    /// <returns>Returns the dto with a modified Foo property</returns>
    public static SamplePackageDto ToUpper(this SamplePackageDto dto)
    {
        return new SamplePackageDto { Foo = dto.Foo.ToUpper() };
    }
}

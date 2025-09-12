using Arbeidstilsynet.Common.SamplePackage.Implementation;
using Arbeidstilsynet.Common.SamplePackage.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Arbeidstilsynet.Common.SamplePackage.DependencyInjection;

/// <summary>
/// Extensions for Dependency Injection.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers an implementation av <see cref="ISamplePackage"/> in <paramref name="services"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to register the service in.</param>
    /// <returns><see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddSamplePackage(this IServiceCollection services)
    {
        services.AddSingleton<ISamplePackage, SamplePackageImplementation>();

        return services;
    }
}

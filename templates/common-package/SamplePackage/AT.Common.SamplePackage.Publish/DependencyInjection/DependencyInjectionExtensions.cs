using Arbeidstilsynet.Common.SamplePackage.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Arbeidstilsynet.Common.SamplePackage.DependencyInjection;

/// <summary>
/// Extensions for Dependency Injection.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registrerer en implementasjon av ISamplePackage i den spesifiserte <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> som tjenesten skal legges til i.</param>
    /// <returns><see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddSamplePackage(this IServiceCollection services)
    {
        services.AddSingleton<ISamplePackage, SamplePackageImplementation>();

        return services;
    }
}

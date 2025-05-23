using Arbeidstilsynet.Common.SamplePackage;
using Arbeidstilsynet.Common.SamplePackage.Adapters;
using Arbeidstilsynet.Common.SamplePackage.Ports;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Retry;

namespace Arbeidstilsynet.Common.SamplePackage.Adapters.DependencyInjection;

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

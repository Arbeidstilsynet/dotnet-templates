# Introduction

Introction to SamplePackage.

## 📖 Description

Describe SamplePackage.

## 🧑‍💻 Usage

Add to your service collection:

```csharp
public static IServiceCollection AddServices
    (
        this IServiceCollection services,
        DatabaseConfiguration databaseConfiguration
    ) {
        services.AddSamplePackage();
        return services;
    }
```

Inject into your class:

```csharp
public class MyService
{
    private readonly ISamplePackage _samplePackage;

    public MyService(ISamplePackage samplePackage)
    {
        _samplePackage = samplePackage;
    }

    public async Task DoSomething()
    {
        var result = await _samplePackage.Get();
        // Use result...
    }
}
```

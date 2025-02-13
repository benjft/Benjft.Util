using System.Diagnostics.CodeAnalysis;
using Benjft.Util.DependencyInjection.Attributes;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable InconsistentNaming

namespace Benjft.Util.DependencyInjection.Tests.Attributes.Examples;

[Service]
public class ServiceAttributeExample_DefaultLifetime;

[Service(ServiceLifetime.Singleton)]
public class ServiceAttributeExample_SingletonLifetime;

[Service(ServiceLifetime.Scoped)]
public class ServiceAttributeExample_ScopedLifetime;

[Service(ServiceLifetime.Transient)]
public class ServiceAttributeExample_TransientLifetime;

[Service(FactoryMethod = nameof(Create))]
public class ServiceAttributeExample_FactoryMethod {
    [ExcludeFromCodeCoverage]
    public static ServiceAttributeExample_FactoryMethod Create(IServiceProvider _) => new();
}

[Service(FactoryMethod = nameof(Create))]
public class ServiceAttributeExample_FactoryMethodHasWrongSignature {
    [ExcludeFromCodeCoverage]
    public static ServiceAttributeExample_FactoryMethodHasWrongSignature Create(string _) => new();
}

[Service(FactoryMethod = "Create")]
public class ServiceAttributeExample_FactoryMethodMissing;

[Service(FactoryMethod = nameof(Create))]
public class ServiceAttributeExample_FactoryMethodNotStatic {
    [ExcludeFromCodeCoverage]
    public ServiceAttributeExample_FactoryMethodNotStatic Create(IServiceProvider _) => new();
}

[Service(ServiceKey = "Key1")]
[Service(ServiceKey = "Key2")]
public class ServiceAttributeExample_Keyed;

[Service(ServiceKey = "Key1", Order = 2)]
[Service(ServiceKey = "Key2", Order = 1)]
public class ServiceAttributeExample_KeyedOrdered;

[Service(FactoryMethod = nameof(Create), ServiceKey = "Key1")]
[Service(FactoryMethod = nameof(Create), ServiceKey = "Key2")]
public class ServiceAttributeExample_KeyedFactoryMethod {
    [ExcludeFromCodeCoverage]
    public static ServiceAttributeExample_KeyedFactoryMethod Create(IServiceProvider _, object? key) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = "Key1")]
[Service(FactoryMethod = nameof(Create), ServiceKey = "Key2")]
public class ServiceAttributeExample_KeyedFactoryMethodHasWrongSignature {
    [ExcludeFromCodeCoverage]
    public static ServiceAttributeExample_KeyedFactoryMethodHasWrongSignature Create(IServiceProvider _) => new();
}

[SingletonService]
public class SingletonServiceAttributeExample;

[ScopedService]
public class ScopedServiceAttributeExample;

[TransientService]
public class TransientServiceAttributeExample;
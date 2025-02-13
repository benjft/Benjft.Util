using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceAttribute() : Attribute() {
    public ServiceAttribute(ServiceLifetime lifetime) : this() {
        Lifetime = lifetime;
    }
    
    public ServiceLifetime? Lifetime { get; init; } = null;

    public string? FactoryMethod { get; init; }
    public object? ServiceKey { get; init; }
    public int Order { get; init; } = 0;
}

public class TransientServiceAttribute() : ServiceAttribute(ServiceLifetime.Transient);
public class ScopedServiceAttribute() : ServiceAttribute(ServiceLifetime.Scoped);
public class SingletonServiceAttribute() : ServiceAttribute(ServiceLifetime.Singleton);
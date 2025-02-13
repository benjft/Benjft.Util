using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ServiceFactoryAttribute() : Attribute {
    public ServiceFactoryAttribute(Type serviceType) : this() {
        ServiceTypeOverride = serviceType;
    }
    
    public ServiceFactoryAttribute(ServiceLifetime lifetime) : this() {
        Lifetime = lifetime;
    }
    
    
    public ServiceFactoryAttribute(Type serviceType, ServiceLifetime lifetime) : this() {
        ServiceTypeOverride = serviceType;
        Lifetime = lifetime;
    }
    
    public Type? ServiceTypeOverride { get; init; } = null;
    public ServiceLifetime? Lifetime { get; init; } = null;
    public object? ServiceKey { get; init; } = null;
    public int Order { get; init; } = 0;
}

public class TransientServiceFactoryAttribute : ServiceFactoryAttribute {
    public TransientServiceFactoryAttribute() : base(ServiceLifetime.Transient) { }
    public TransientServiceFactoryAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Transient) { }
}

public class ScopedServiceFactoryAttribute : ServiceFactoryAttribute {
    public ScopedServiceFactoryAttribute() : base(ServiceLifetime.Scoped) { }
    public ScopedServiceFactoryAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Scoped) { }
}

public class SingletonServiceFactoryAttribute : ServiceFactoryAttribute {
    public SingletonServiceFactoryAttribute() : base(ServiceLifetime.Singleton) { }
    public SingletonServiceFactoryAttribute(Type serviceType) : base(serviceType, ServiceLifetime.Singleton) { }
}

public class TransientServiceFactoryAttribute<TService>() : TransientServiceFactoryAttribute(typeof(TService));
public class ScopedServiceFactoryAttribute<TService>() : ScopedServiceFactoryAttribute(typeof(TService));
public class SingletonServiceFactoryAttribute<TService>() : SingletonServiceFactoryAttribute(typeof(TService));
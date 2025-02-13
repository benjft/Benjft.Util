using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Attributes;

public class ImplementsServiceAttribute : ServiceAttribute {
    public Type ServiceType { get; }

    public ImplementsServiceAttribute(Type serviceType, ServiceLifetime lifetime) : base(lifetime) {
        ServiceType = serviceType;
    }

    public ImplementsServiceAttribute(Type serviceType) : base() {
        ServiceType = serviceType;
    }
}

public class ImplementsServiceAttribute<TService> : ImplementsServiceAttribute {
    public ImplementsServiceAttribute() : base(typeof(TService)) { }
    public ImplementsServiceAttribute(ServiceLifetime lifetime) : base(typeof(TService), lifetime) { }
}

public class ImplementsTransientServiceAttribute(Type serviceType) : ImplementsServiceAttribute(serviceType, ServiceLifetime.Transient);
public class ImplementsScopedServiceAttribute(Type serviceType) : ImplementsServiceAttribute(serviceType, ServiceLifetime.Scoped);
public class ImplementsSingletonServiceAttribute(Type serviceType) : ImplementsServiceAttribute(serviceType, ServiceLifetime.Singleton);

public class ImplementsTransientServiceAttribute<TService>() : ImplementsServiceAttribute<TService>(ServiceLifetime.Transient);
public class ImplementsScopedServiceAttribute<TService>() : ImplementsServiceAttribute<TService>(ServiceLifetime.Scoped);
public class ImplementsSingletonServiceAttribute<TService>() : ImplementsServiceAttribute<TService>(ServiceLifetime.Singleton);

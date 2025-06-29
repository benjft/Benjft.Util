using Benjft.Util.DependencyInjection.Attributes;

namespace Benjft.Util.DependencyInjection.Tests.Attributes;

public enum ServiceKeyEnum { A, B, C }
[Service()]
public class ServiceAttributeTest_DefaultLifetime_NoFactory_NoKey { 
}

[Service(ServiceKey = 1)]
public class ServiceAttributeTest_DefaultLifetime_NoFactory_IntKey { 
}

[Service(ServiceKey = "serviceKey")]
public class ServiceAttributeTest_DefaultLifetime_NoFactory_StringKey { 
}

[Service(ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_DefaultLifetime_NoFactory_EnumKey { 
}

[Service(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_DefaultLifetime_ValidFactory_NoKey { 
    public ServiceAttributeTest_DefaultLifetime_ValidFactory_NoKey Create(IServiceProvider serviceProvider) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_DefaultLifetime_ValidFactory_IntKey { 
    public ServiceAttributeTest_DefaultLifetime_ValidFactory_IntKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_DefaultLifetime_ValidFactory_StringKey { 
    public ServiceAttributeTest_DefaultLifetime_ValidFactory_StringKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_DefaultLifetime_ValidFactory_EnumKey { 
    public ServiceAttributeTest_DefaultLifetime_ValidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[Service(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_DefaultLifetime_InvalidFactory_NoKey { 
    public ServiceAttributeTest_DefaultLifetime_InvalidFactory_NoKey Create(IServiceProvider serviceProvider, object invalidExtraParameter) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_DefaultLifetime_InvalidFactory_IntKey { 
    public ServiceAttributeTest_DefaultLifetime_InvalidFactory_IntKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_DefaultLifetime_InvalidFactory_StringKey { 
    public ServiceAttributeTest_DefaultLifetime_InvalidFactory_StringKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_DefaultLifetime_InvalidFactory_EnumKey { 
    public ServiceAttributeTest_DefaultLifetime_InvalidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[SingletonService()]
public class ServiceAttributeTest_Singleton_NoFactory_NoKey { 
}

[SingletonService(ServiceKey = 1)]
public class ServiceAttributeTest_Singleton_NoFactory_IntKey { 
}

[SingletonService(ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Singleton_NoFactory_StringKey { 
}

[SingletonService(ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Singleton_NoFactory_EnumKey { 
}

[SingletonService(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_Singleton_ValidFactory_NoKey { 
    public ServiceAttributeTest_Singleton_ValidFactory_NoKey Create(IServiceProvider serviceProvider) => new();
}

[SingletonService(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_Singleton_ValidFactory_IntKey { 
    public ServiceAttributeTest_Singleton_ValidFactory_IntKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[SingletonService(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Singleton_ValidFactory_StringKey { 
    public ServiceAttributeTest_Singleton_ValidFactory_StringKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[SingletonService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Singleton_ValidFactory_EnumKey { 
    public ServiceAttributeTest_Singleton_ValidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[SingletonService(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_Singleton_InvalidFactory_NoKey { 
    public ServiceAttributeTest_Singleton_InvalidFactory_NoKey Create(IServiceProvider serviceProvider, object invalidExtraParameter) => new();
}

[SingletonService(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_Singleton_InvalidFactory_IntKey { 
    public ServiceAttributeTest_Singleton_InvalidFactory_IntKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[SingletonService(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Singleton_InvalidFactory_StringKey { 
    public ServiceAttributeTest_Singleton_InvalidFactory_StringKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[SingletonService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Singleton_InvalidFactory_EnumKey { 
    public ServiceAttributeTest_Singleton_InvalidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[ScopedService()]
public class ServiceAttributeTest_Scoped_NoFactory_NoKey { 
}

[ScopedService(ServiceKey = 1)]
public class ServiceAttributeTest_Scoped_NoFactory_IntKey { 
}

[ScopedService(ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Scoped_NoFactory_StringKey { 
}

[ScopedService(ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Scoped_NoFactory_EnumKey { 
}

[ScopedService(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_Scoped_ValidFactory_NoKey { 
    public ServiceAttributeTest_Scoped_ValidFactory_NoKey Create(IServiceProvider serviceProvider) => new();
}

[ScopedService(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_Scoped_ValidFactory_IntKey { 
    public ServiceAttributeTest_Scoped_ValidFactory_IntKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[ScopedService(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Scoped_ValidFactory_StringKey { 
    public ServiceAttributeTest_Scoped_ValidFactory_StringKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[ScopedService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Scoped_ValidFactory_EnumKey { 
    public ServiceAttributeTest_Scoped_ValidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[ScopedService(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_Scoped_InvalidFactory_NoKey { 
    public ServiceAttributeTest_Scoped_InvalidFactory_NoKey Create(IServiceProvider serviceProvider, object invalidExtraParameter) => new();
}

[ScopedService(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_Scoped_InvalidFactory_IntKey { 
    public ServiceAttributeTest_Scoped_InvalidFactory_IntKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[ScopedService(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Scoped_InvalidFactory_StringKey { 
    public ServiceAttributeTest_Scoped_InvalidFactory_StringKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[ScopedService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Scoped_InvalidFactory_EnumKey { 
    public ServiceAttributeTest_Scoped_InvalidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[TransientService()]
public class ServiceAttributeTest_Transient_NoFactory_NoKey { 
}

[TransientService(ServiceKey = 1)]
public class ServiceAttributeTest_Transient_NoFactory_IntKey { 
}

[TransientService(ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Transient_NoFactory_StringKey { 
}

[TransientService(ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Transient_NoFactory_EnumKey { 
}

[TransientService(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_Transient_ValidFactory_NoKey { 
    public ServiceAttributeTest_Transient_ValidFactory_NoKey Create(IServiceProvider serviceProvider) => new();
}

[TransientService(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_Transient_ValidFactory_IntKey { 
    public ServiceAttributeTest_Transient_ValidFactory_IntKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[TransientService(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Transient_ValidFactory_StringKey { 
    public ServiceAttributeTest_Transient_ValidFactory_StringKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[TransientService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Transient_ValidFactory_EnumKey { 
    public ServiceAttributeTest_Transient_ValidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key) => new();
}

[TransientService(FactoryMethod = nameof(Create))]
public class ServiceAttributeTest_Transient_InvalidFactory_NoKey { 
    public ServiceAttributeTest_Transient_InvalidFactory_NoKey Create(IServiceProvider serviceProvider, object invalidExtraParameter) => new();
}

[TransientService(FactoryMethod = nameof(Create), ServiceKey = 1)]
public class ServiceAttributeTest_Transient_InvalidFactory_IntKey { 
    public ServiceAttributeTest_Transient_InvalidFactory_IntKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[TransientService(FactoryMethod = nameof(Create), ServiceKey = "serviceKey")]
public class ServiceAttributeTest_Transient_InvalidFactory_StringKey { 
    public ServiceAttributeTest_Transient_InvalidFactory_StringKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}

[TransientService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.A)]
public class ServiceAttributeTest_Transient_InvalidFactory_EnumKey { 
    public ServiceAttributeTest_Transient_InvalidFactory_EnumKey Create(IServiceProvider serviceProvider, object? key, object invalidExtraParameter) => new();
}


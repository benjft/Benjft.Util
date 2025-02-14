using Benjft.Util.DependencyInjection.Attributes;
using Benjft.Util.DependencyInjection.Exceptions;
using Benjft.Util.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Tests.Attributes.ServiceAttributeTests;

public class KeyedServiceTests {
    [Fact]
    public void ServiceAttribute_CreatesKeyedServices_WhenUsedOnClass() {
        var serviceType = typeof(KeyedServiceAttributeExample);
        
        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.True(actualService.IsKeyedService);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }
    
    [Fact]
    public void ServiceAttribute_ThrowsException_WhenUsedOnAbstractClass() {
        var serviceType = typeof(AbstractKeyedServiceAttributeExample);

        Assert.Throws<InvalidServiceTypeException>(() => serviceType.GetServicesFromAttributes().ToList());
    }
    
    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void ServiceAttribute_UsesDefaultLifetime_WhenNoLifetimeIsSet(ServiceLifetime defaultLifetime) {
        var serviceType = typeof(KeyedServiceAttributeExample);
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(actualService.Lifetime, defaultLifetime);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }
    
    [Theory]
    [InlineData(typeof(TransientKeyedServiceAttributeExample), ServiceLifetime.Transient)]
    [InlineData(typeof(ScopedKeyedServiceAttributeExample), ServiceLifetime.Scoped)]
    [InlineData(typeof(SingletonKeyedServiceAttributeExample), ServiceLifetime.Singleton)]
    public void ServiceAttribute_UsesLifetimeSetInTheAttribute_WhenLifetimeIsSet(
        Type serviceType,
        ServiceLifetime expectedLifetime) {
        var defaultLifetime = expectedLifetime != ServiceLifetime.Singleton ? ServiceLifetime.Singleton : ServiceLifetime.Transient;
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(actualService.Lifetime, expectedLifetime);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }

    [Fact]
    public void ServiceAttribute_CreatesMultipleKeyedServices_WhenMultipleKeysAreSet() {
        var serviceType = typeof(KeyedServiceAttributeMultipleExample);

        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(3, services.Length);
        Assert.Contains(ServiceKeyEnum.Key1, services.Select(s => s.ServiceKey));
        Assert.Contains(ServiceKeyEnum.Key2, services.Select(s => s.ServiceKey));
        Assert.Contains(ServiceKeyEnum.Key3, services.Select(s => s.ServiceKey));
    }

    [Fact]
    public void ServiceAttribute_AddsServicesInOrder_WhenOrderIsSpecified() {
        var serviceType = typeof(KeyedServiceAttributeMultipleOrderedExample);

        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(3, services.Length);
        Assert.Equal(ServiceKeyEnum.Key2, services[0].ServiceKey);
        Assert.Equal(ServiceKeyEnum.Key3, services[1].ServiceKey);
        Assert.Equal(ServiceKeyEnum.Key1, services[2].ServiceKey);
    }

    #region Test Classes
    
    [Service(ServiceKey = ServiceKeyEnum.Key1)]
    private class KeyedServiceAttributeExample;
    
    [Service(ServiceKey = ServiceKeyEnum.Key1)]
    private abstract class AbstractKeyedServiceAttributeExample;

    [TransientService(ServiceKey = ServiceKeyEnum.Key1)]
    private class TransientKeyedServiceAttributeExample;

    [ScopedService(ServiceKey = ServiceKeyEnum.Key1)]
    private class ScopedKeyedServiceAttributeExample;

    [SingletonService(ServiceKey = ServiceKeyEnum.Key1)]
    private class SingletonKeyedServiceAttributeExample;

    [Service(ServiceKey = ServiceKeyEnum.Key1)]
    [Service(ServiceKey = ServiceKeyEnum.Key2)]
    [Service(ServiceKey = ServiceKeyEnum.Key3)]
    private class KeyedServiceAttributeMultipleExample;

    [Service(ServiceKey = ServiceKeyEnum.Key1, Order = 3)]
    [Service(ServiceKey = ServiceKeyEnum.Key2, Order = 1)]
    [Service(ServiceKey = ServiceKeyEnum.Key3, Order = 2)]
    private class KeyedServiceAttributeMultipleOrderedExample;

    private enum ServiceKeyEnum {
        Key1,
        Key2,
        Key3,
    }
    
    #endregion Test Classes
}

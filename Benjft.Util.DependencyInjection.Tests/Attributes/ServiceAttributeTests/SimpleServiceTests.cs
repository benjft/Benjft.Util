using Benjft.Util.DependencyInjection.Attributes;
using Benjft.Util.DependencyInjection.Exceptions;
using Benjft.Util.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Tests.Attributes.ServiceAttributeTests;

public class SimpleServiceTests {
    [Fact]
    public void ServiceAttribute_AddsService_WhenUsedOnClass() {
        var serviceType = typeof(ServiceAttributeExample);

        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.Equal(serviceType, actualService.ServiceType);
    }
    
    [Fact]
    public void ServiceAttribute_ThrowsException_WhenUsedOnAbstractClass() {
        var serviceType = typeof(AbstractServiceAttributeExample);

        Assert.Throws<InvalidServiceTypeException>(() => serviceType.GetServicesFromAttributes().ToList());
    }
    
    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void ServiceAttribute_UsesDefaultLifetime_WhenNoLifetimeIsSet(ServiceLifetime defaultLifetime) {
        var serviceType = typeof(ServiceAttributeExample);
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(actualService.Lifetime, defaultLifetime);
    }
    
    [Theory]
    [InlineData(typeof(TransientServiceAttributeExample), ServiceLifetime.Transient)]
    [InlineData(typeof(ScopedServiceAttributeExample), ServiceLifetime.Scoped)]
    [InlineData(typeof(SingletonServiceAttributeExample), ServiceLifetime.Singleton)]
    public void ServiceAttribute_UsesLifetimeSetInTheAttribute_WhenLifetimeIsSet(
        Type serviceType,
        ServiceLifetime expectedLifetime) {
        var defaultLifetime = expectedLifetime != ServiceLifetime.Singleton ? ServiceLifetime.Singleton : ServiceLifetime.Transient;
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(actualService.Lifetime, expectedLifetime);
    }

    #region Test Classes

    [Service]
    private class ServiceAttributeExample;

    [Service]
    private abstract class AbstractServiceAttributeExample;

    [TransientService]
    private class TransientServiceAttributeExample;

    [ScopedService]
    private class ScopedServiceAttributeExample;

    [SingletonService]
    private class SingletonServiceAttributeExample;

    #endregion Test Classes
}

using Benjft.Util.DependencyInjection.Exceptions;
using Benjft.Util.DependencyInjection.Extensions;
using Benjft.Util.DependencyInjection.Tests.Attributes.Examples;
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Tests.Attributes;

public class ServiceAttributeTests {
    
    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void ServiceAttribute_ShouldUseTheDefaultLifetime_WhenNoLifetimeIsSet(ServiceLifetime defaultLifetime) {
        var serviceType = typeof(ServiceAttributeExample_DefaultLifetime);
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(defaultLifetime, actualService.Lifetime);
    }
     
    [Theory]
    [InlineData(typeof(ServiceAttributeExample_SingletonLifetime), ServiceLifetime.Singleton)]
    [InlineData(typeof(ServiceAttributeExample_ScopedLifetime), ServiceLifetime.Scoped)]
    [InlineData(typeof(ServiceAttributeExample_TransientLifetime), ServiceLifetime.Transient)]
    public void ServiceAttribute_ShouldUseTheLifetimeSetInTheAttribute_WhenLifetimeIsSet(
        Type serviceType,
        ServiceLifetime expectedLifetime
        ) {
        var defaultLifetime = expectedLifetime != ServiceLifetime.Singleton ? ServiceLifetime.Singleton : ServiceLifetime.Transient;
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(expectedLifetime, actualService.Lifetime);
    }
    
    [Fact]
    public void ServiceAttribute_ShouldUseTheFactoryMethod_WhenFactoryMethodIsSet() {
        var serviceType = typeof(ServiceAttributeExample_FactoryMethod);
        
        var factory = GetServiceFactory(serviceType, nameof(ServiceAttributeExample_FactoryMethod.Create));
        
        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.Equal(factory, actualService.ImplementationFactory);
    }

    [Theory]
    [InlineData(typeof(ServiceAttributeExample_FactoryMethodHasWrongSignature),
                typeof(InvalidFactoryMethodException),
                typeof(FactoryMethodHasWrongSignatureException))]
    [InlineData(typeof(ServiceAttributeExample_FactoryMethodMissing),
                typeof(FactoryMethodNotFoundException),
                null)]
    [InlineData(typeof(ServiceAttributeExample_FactoryMethodNotStatic),
                typeof(InvalidFactoryMethodException),
                typeof(FactoryMethodNotStaticException))]
    [InlineData(typeof(ServiceAttributeExample_KeyedFactoryMethodHasWrongSignature),
                typeof(InvalidFactoryMethodException),
                typeof(FactoryMethodHasWrongSignatureException))]
    [InlineData(typeof(ServiceAttributeExample_FactoryMethodHasWrongServiceType),
                typeof(InvalidServiceTypeException),
                null)]
    public void ServiceAttribute_ShouldThrowAnException_WhenFactoryMethodIsInvalid(Type serviceType, Type exceptionType, Type? innerExceptionType) {
        var actualException = Assert.Throws(exceptionType, () => serviceType.GetServicesFromAttributes().ToList());
        
        if (innerExceptionType != null) {
            Assert.IsType(innerExceptionType, actualException.InnerException);
        }
    }

    [Theory]
    [InlineData(typeof(ServiceAttributeExample_Keyed))]
    [InlineData(typeof(ServiceAttributeExample_KeyedFactoryMethod))]
    public void ServiceAttribute_ShouldUseKeyedServices_WhenKeysAreProvided(Type serviceType) {        
        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(2, services.Length);
        Assert.Contains(services, x => Equals(x.ServiceKey, "Key1"));
        Assert.Contains(services, x => Equals(x.ServiceKey, "Key2"));
    }

    [Fact]
    public void ServiceAttribute_ShouldAddServicesInTheCorrectOrder_WhenOrderIsProvided() {
        var serviceType = typeof(ServiceAttributeExample_KeyedOrdered);
        
        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(2, services.Length);
        Assert.Equal("Key2", services[0].ServiceKey);
        Assert.Equal("Key1", services[1].ServiceKey);
    }

    [Theory]
    [InlineData(typeof(SingletonServiceAttributeExample), ServiceLifetime.Singleton)]
    [InlineData(typeof(ScopedServiceAttributeExample), ServiceLifetime.Scoped)]
    [InlineData(typeof(TransientServiceAttributeExample), ServiceLifetime.Transient)]
    public void ExtendedServiceAttributes_ShouldRegisterAsTheCorrectLifetime_WhenUsedOnAClass(
        Type serviceType,
        ServiceLifetime expectedLifetime) {
        var services = serviceType.GetServicesFromAttributes(expectedLifetime);
        var actualService = Assert.Single(services);
        Assert.Equal(expectedLifetime, actualService.Lifetime);
    }
    
    private static Func<IServiceProvider, object> GetServiceFactory(Type type, string methodName) =>
        type.GetMethod(methodName)!.CreateDelegate<Func<IServiceProvider, object>>();
}

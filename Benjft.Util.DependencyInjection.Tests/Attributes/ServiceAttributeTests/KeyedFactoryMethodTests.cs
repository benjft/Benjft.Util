using System.Diagnostics.CodeAnalysis;
using Benjft.Util.DependencyInjection.Attributes;
using Benjft.Util.DependencyInjection.Exceptions;
using Benjft.Util.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Tests.Attributes.ServiceAttributeTests;

public class KeyedFactoryMethodTests {
    
    [Fact]
    public void ServiceAttribute_AddsService_WhenUsedOnClass() {
        var serviceType = typeof(ServiceFactoryAttributeExample);

        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.Equal(serviceType, actualService.ServiceType);
        var factoryMethod = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeExample.Create));
        Assert.Equal(factoryMethod, actualService.KeyedImplementationFactory);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }
    
    [Fact]
    public void ServiceAttribute_AddsService_WhenUsedOnAbstractClassIfFactoryMethodExists() {
        var serviceType = typeof(AbstractServiceFactoryAttributeExample);

        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.Equal(serviceType, actualService.ServiceType);
        var factoryMethod = GetFactoryMethod(serviceType, nameof(AbstractServiceFactoryAttributeExample.Create));
        Assert.Equal(factoryMethod, actualService.KeyedImplementationFactory);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }
    
    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void ServiceAttribute_UsesDefaultLifetime_WhenNoLifetimeIsSet(ServiceLifetime defaultLifetime) {
        var serviceType = typeof(ServiceFactoryAttributeExample);
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(actualService.Lifetime, defaultLifetime);
        var factoryMethod = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeExample.Create));
        Assert.Equal(factoryMethod, actualService.KeyedImplementationFactory);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }
    
    [Theory]
    [InlineData(typeof(TransientServiceFactoryAttributeExample), ServiceLifetime.Transient)]
    [InlineData(typeof(ScopedServiceFactoryAttributeExample), ServiceLifetime.Scoped)]
    [InlineData(typeof(SingletonServiceFactoryAttributeExample), ServiceLifetime.Singleton)]
    public void ServiceAttribute_UsesLifetimeSetInTheAttribute_WhenLifetimeIsSet(
        Type serviceType,
        ServiceLifetime expectedLifetime) {
        var defaultLifetime = expectedLifetime != ServiceLifetime.Singleton ? ServiceLifetime.Singleton : ServiceLifetime.Transient;
        
        var services = serviceType.GetServicesFromAttributes(defaultLifetime);
        
        var actualService = Assert.Single(services);
        Assert.Equal(actualService.Lifetime, expectedLifetime);
        var factoryMethod = GetFactoryMethod(serviceType, "Create");
        Assert.Equal(factoryMethod, actualService.KeyedImplementationFactory);
        Assert.Equal(ServiceKeyEnum.Key1, actualService.ServiceKey);
    }

    [Theory]
    [InlineData(typeof(ServiceFactoryAttributeExampleIncorrectSignature),
                typeof(InvalidFactoryMethodException),
                typeof(FactoryMethodHasWrongSignatureException))]
    [InlineData(typeof(ServiceFactoryAttributeExampleFactoryDoesNotExist),
                typeof(FactoryMethodNotFoundException),
                null)]
    [InlineData(typeof(ServiceFactoryAttributeExampleFactoryNotStatic),
                typeof(InvalidFactoryMethodException),
                typeof(FactoryMethodNotStaticException))]
    [InlineData(typeof(ServiceFactoryAttributeExampleWrongReturnType),
                typeof(InvalidServiceTypeException),
                null)]
    public void ServiceAttribute_ShouldThrowAnException_WhenFactoryMethodIsInvalid(Type serviceType, Type exceptionType, Type? innerExceptionType) {
        var actualException = Assert.Throws(exceptionType, () => serviceType.GetServicesFromAttributes().ToList());
        
        if (innerExceptionType != null) {
            Assert.IsType(innerExceptionType, actualException.InnerException);
        }
    }

    [Fact]
    public void ServiceAttribute_CreatesMultipleKeyedServices_WhenMultipleKeysAreSet() {
        var serviceType = typeof(ServiceFactoryAttributeMultipleExample);

        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(3, services.Length);
        Assert.Contains(ServiceKeyEnum.Key1, services.Select(s => s.ServiceKey));
        Assert.Contains(ServiceKeyEnum.Key2, services.Select(s => s.ServiceKey));
        Assert.Contains(ServiceKeyEnum.Key3, services.Select(s => s.ServiceKey));
        Assert.All(services, s => Assert.Equal(serviceType, s.ServiceType));
        var factoryMethod = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeMultipleExample.Create));
        Assert.All(services, s => Assert.Equal(factoryMethod, s.KeyedImplementationFactory));
    }

    [Fact]
    public void ServiceAttribute_MayUseMultipleFactories_WhenMultipleKeysAreSet() {
        var serviceType = typeof(ServiceFactoryAttributeMultipleFactoriesExample);

        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(3, services.Length);
        var factoryMethod1 = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeMultipleFactoriesExample.Create1));
        Assert.Contains((ServiceKeyEnum.Key1, factoryMethod1), services.Select(s => (s.ServiceKey, s.KeyedImplementationFactory)));
        var factoryMethod2 = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeMultipleFactoriesExample.Create2));
        Assert.Contains((ServiceKeyEnum.Key2, factoryMethod2), services.Select(s => (s.ServiceKey, s.KeyedImplementationFactory)));
        var factoryMethod3 = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeMultipleFactoriesExample.Create3));
        Assert.Contains((ServiceKeyEnum.Key3, factoryMethod3), services.Select(s => (s.ServiceKey, s.KeyedImplementationFactory)));
        Assert.All(services, s => Assert.Equal(serviceType, s.ServiceType));
    }

    [Fact]
    public void ServiceAttribute_AddsServicesInOrder_WhenOrderIsSpecified() {
        var serviceType = typeof(ServiceFactoryAttributeMultipleOrderedExample);

        var services = serviceType.GetServicesFromAttributes().ToArray();
        
        Assert.Equal(3, services.Length);
        Assert.Equal(ServiceKeyEnum.Key1, services[2].ServiceKey);
        Assert.Equal(ServiceKeyEnum.Key2, services[0].ServiceKey);
        Assert.Equal(ServiceKeyEnum.Key3, services[1].ServiceKey);
        Assert.All(services, s => Assert.Equal(serviceType, s.ServiceType));
        var factoryMethod = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeMultipleOrderedExample.Create));
        Assert.All(services, s => Assert.Equal(factoryMethod, s.KeyedImplementationFactory));
    }
    
    private static Func<IServiceProvider, object?, object> GetFactoryMethod(Type serviceType, string factoryMethodName) {
        return serviceType.GetMethod(factoryMethodName)!.CreateDelegate<Func<IServiceProvider, object?, object>>();
    }

    #region Test Classes
    
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class ServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeExample Create(IServiceProvider serviceProvider, object? key) => new();
    }

    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private abstract class AbstractServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static AbstractServiceFactoryAttributeExample Create(IServiceProvider serviceProvider, object? key) => throw new NotImplementedException();
    }

    [TransientService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class TransientServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static TransientServiceFactoryAttributeExample Create(IServiceProvider serviceProvider, object? key) => new();
    }

    [ScopedService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class ScopedServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static ScopedServiceFactoryAttributeExample Create(IServiceProvider serviceProvider, object? key) => new();
    }

    [SingletonService(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class SingletonServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static SingletonServiceFactoryAttributeExample Create(IServiceProvider serviceProvider, object? key) => new();
    }
    
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class ServiceFactoryAttributeExampleWrongReturnType {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeExample Create(IServiceProvider serviceProvider, object? key) => new();
    }

    [Service(FactoryMethod = "Create", ServiceKey = ServiceKeyEnum.Key1)]
    private class ServiceFactoryAttributeExampleFactoryDoesNotExist;
    
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class ServiceFactoryAttributeExampleFactoryNotStatic {
        [ExcludeFromCodeCoverage]
        public ServiceFactoryAttributeExampleFactoryNotStatic Create(IServiceProvider serviceProvider, object? key) => new();
    }
    
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    private class ServiceFactoryAttributeExampleIncorrectSignature {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeExampleIncorrectSignature Create(IServiceProvider serviceProvider) => new();
    }
    
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1)]
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key2)]
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key3)]
    private class ServiceFactoryAttributeMultipleExample {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeMultipleExample Create(IServiceProvider serviceProvider, object? key) => new();
    }
    
    [Service(FactoryMethod = nameof(Create1), ServiceKey = ServiceKeyEnum.Key1)]
    [Service(FactoryMethod = nameof(Create2), ServiceKey = ServiceKeyEnum.Key2)]
    [Service(FactoryMethod = nameof(Create3), ServiceKey = ServiceKeyEnum.Key3)]
    private class ServiceFactoryAttributeMultipleFactoriesExample {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeMultipleFactoriesExample Create1(IServiceProvider serviceProvider, object? key) => new();
        public static ServiceFactoryAttributeMultipleFactoriesExample Create2(IServiceProvider serviceProvider, object? key) => new();
        public static ServiceFactoryAttributeMultipleFactoriesExample Create3(IServiceProvider serviceProvider, object? key) => new();
    }
    
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key1, Order = 3)]
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key2, Order = 1)]
    [Service(FactoryMethod = nameof(Create), ServiceKey = ServiceKeyEnum.Key3, Order = 2)]
    private class ServiceFactoryAttributeMultipleOrderedExample {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeMultipleOrderedExample Create(IServiceProvider serviceProvider, object? key) => new();
    }

    private enum ServiceKeyEnum {
        Key1,
        Key2,
        Key3,
    }
    
    #endregion Test Classes
}

using System.Diagnostics.CodeAnalysis;
using Benjft.Util.DependencyInjection.Attributes;
using Benjft.Util.DependencyInjection.Exceptions;
using Benjft.Util.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Tests.Attributes.ServiceAttributeTests;

public class FactoryMethodTests {
    
    [Fact]
    public void ServiceAttribute_AddsService_WhenUsedOnClass() {
        var serviceType = typeof(ServiceFactoryAttributeExample);

        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.Equal(serviceType, actualService.ServiceType);
        var factoryMethod = GetFactoryMethod(serviceType, nameof(ServiceFactoryAttributeExample.Create));
        Assert.Equal(factoryMethod, actualService.ImplementationFactory);
    }
    
    [Fact]
    public void ServiceAttribute_AddsService_WhenUsedOnAbstractClassIfFactoryMethodExists() {
        var serviceType = typeof(AbstractServiceFactoryAttributeExample);

        var services = serviceType.GetServicesFromAttributes();
        
        var actualService = Assert.Single(services);
        Assert.Equal(serviceType, actualService.ServiceType);
        var factoryMethod = GetFactoryMethod(serviceType, nameof(AbstractServiceFactoryAttributeExample.Create));
        Assert.Equal(factoryMethod, actualService.ImplementationFactory);
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
        Assert.Equal(factoryMethod, actualService.ImplementationFactory);
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
        Assert.Equal(factoryMethod, actualService.ImplementationFactory);
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

    private static Func<IServiceProvider, object> GetFactoryMethod(Type serviceType, string factoryMethodName) {
        return serviceType.GetMethod(factoryMethodName)!.CreateDelegate<Func<IServiceProvider, object>>();
    }

    #region Test Classes
    
    [Service(FactoryMethod = nameof(Create))]
    private class ServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeExample Create(IServiceProvider serviceProvider) => new();
    }

    [Service(FactoryMethod = nameof(Create))]
    private abstract class AbstractServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static AbstractServiceFactoryAttributeExample Create(IServiceProvider serviceProvider) => throw new NotImplementedException();
    }

    [TransientService(FactoryMethod = nameof(Create))]
    private class TransientServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static TransientServiceFactoryAttributeExample Create(IServiceProvider serviceProvider) => new();
    }

    [ScopedService(FactoryMethod = nameof(Create))]
    private class ScopedServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static ScopedServiceFactoryAttributeExample Create(IServiceProvider serviceProvider) => new();
    }

    [SingletonService(FactoryMethod = nameof(Create))]
    private class SingletonServiceFactoryAttributeExample {
        [ExcludeFromCodeCoverage]
        public static SingletonServiceFactoryAttributeExample Create(IServiceProvider serviceProvider) => new();
    }
    
    [Service(FactoryMethod = nameof(Create))]
    private class ServiceFactoryAttributeExampleWrongReturnType {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeExample Create(IServiceProvider serviceProvider) => new();
    }

    [Service(FactoryMethod = "Create")]
    private class ServiceFactoryAttributeExampleFactoryDoesNotExist;
    
    [Service(FactoryMethod = nameof(Create))]
    private class ServiceFactoryAttributeExampleFactoryNotStatic {
        [ExcludeFromCodeCoverage]
        public ServiceFactoryAttributeExampleFactoryNotStatic Create(IServiceProvider serviceProvider) => new();
    }
    
    [Service(FactoryMethod = nameof(Create))]
    private class ServiceFactoryAttributeExampleIncorrectSignature {
        [ExcludeFromCodeCoverage]
        public static ServiceFactoryAttributeExampleIncorrectSignature Create() => new();
    }
    
    #endregion Test Classes
}

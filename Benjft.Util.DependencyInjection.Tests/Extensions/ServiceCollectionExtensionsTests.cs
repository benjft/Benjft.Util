using Microsoft.Extensions.DependencyInjection;
using Benjft.Util.DependencyInjection.Extensions;
using Benjft.Util.DependencyInjection.Tests.Helpers;

namespace Benjft.Util.DependencyInjection.Tests.Extensions;

public class ServiceCollectionExtensionsTests {

    [Fact]
    public void ServiceCollectionExtensions_ShouldRegisterServiceFactories_ForEachServiceFactoryAttribute() {
        // Arrange
        var services = new ServiceCollection();
        var expectedServices = new List<ServiceDescriptor> {
            ServiceDescriptor.Singleton(typeof(IExampleFactorySingleton), 
                                        GetMethodDelegate(typeof(StaticFactoryExample), nameof(StaticFactoryExample.GetExampleFactorySingleton))),
            
        };
        
        // Act
        services.AddServicesFromAttributes(typeof(StaticFactoryExample));
        
        // Assert
        Assert.Equal(expectedServices.Count, services.Count);
        Assert.True(services.All(x => expectedServices.Contains(x, ServiceDescriptorComparer.Instance)));
    }

    private static Func<IServiceProvider, object> GetMethodDelegate(Type type, string method) {
        return type.GetMethod(method)!.CreateDelegate<Func<IServiceProvider, object>>();
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection.Tests.Helpers;

public class ServiceDescriptorComparer : IEqualityComparer<ServiceDescriptor> {
    public static ServiceDescriptorComparer Instance { get; } = new();
    
    public bool Equals(ServiceDescriptor? x, ServiceDescriptor? y) {
        if (ReferenceEquals(x, y)) {
            return true;
        }

        if (x is null) {
            return false;
        }

        if (y is null) {
            return false;
        }

        if (x.GetType() != y.GetType()) {
            return false;
        }

        var areEqualWithoutKey = x.Lifetime == y.Lifetime
                              && Equals(x.ServiceKey, y.ServiceKey)
                              && Equals(x.ServiceType, y.ServiceType)
                              && Equals(x.ImplementationType, y.ImplementationType)
                              && Equals(x.ImplementationInstance, y.ImplementationInstance)
                              && Equals(x.ImplementationFactory, y.ImplementationFactory)
                              && x.IsKeyedService == y.IsKeyedService;

        if (!areEqualWithoutKey || !x.IsKeyedService) {
            return areEqualWithoutKey;
        }
        
        return areEqualWithoutKey
            && Equals(x.KeyedImplementationType, y.KeyedImplementationType)
            && Equals(x.KeyedImplementationInstance, y.KeyedImplementationInstance)
            && Equals(x.KeyedImplementationFactory, y.KeyedImplementationFactory);
    }

    public int GetHashCode(ServiceDescriptor obj) {
        var hashCode = new HashCode();
        hashCode.Add((int)obj.Lifetime);
        hashCode.Add(obj.ServiceKey);
        hashCode.Add(obj.ServiceType);
        hashCode.Add(obj.ImplementationType);
        hashCode.Add(obj.KeyedImplementationType);
        hashCode.Add(obj.ImplementationInstance);
        hashCode.Add(obj.KeyedImplementationInstance);
        hashCode.Add(obj.ImplementationFactory);
        hashCode.Add(obj.KeyedImplementationFactory);
        hashCode.Add(obj.IsKeyedService);
        return hashCode.ToHashCode();
    }
}

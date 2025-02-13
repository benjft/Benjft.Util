using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Benjft.Util.DependencyInjection;

internal class ServiceDescriptorWrapper(ServiceDescriptor serviceDescriptor, int order)
    : IComparable<ServiceDescriptorWrapper>, IComparable {
    
    public void Deconstruct(out ServiceDescriptor serviceDescriptor, out int order) {
        serviceDescriptor = ServiceDescriptor;
        order = Order;
    }

    public ServiceDescriptor ServiceDescriptor { get; init; } = serviceDescriptor;
    public int Order { get; init; } = order;

    public int CompareTo(ServiceDescriptorWrapper? other) {
        if (ReferenceEquals(this, other)) {
            return 0;
        }
        if (other is null) {
            return 1;
        }

        var result = Order.CompareTo(other.Order);
        if (result != 0) {
            return result;
        }

        result = string.Compare(
            ServiceDescriptor.ServiceType.FullName,
            other.ServiceDescriptor.ServiceType.FullName,
            StringComparison.Ordinal);
        if (result != 0) {
            return result;
        }

        var isKeyed = ServiceDescriptor.IsKeyedService;
        var otherIsKeyed = other.ServiceDescriptor.IsKeyedService;
        return (isKeyed, otherIsKeyed) switch {
            (false, true) => -1,
            (true, false) => 1,
            (true, true)  => CompareServiceKeys(other),
            _             => 0,
        };
    }

    private int CompareServiceKeys(ServiceDescriptorWrapper other) {
        try {
            return Comparer.Default.Compare(ServiceDescriptor.ServiceKey, other.ServiceDescriptor.ServiceKey);
        } catch (ArgumentException) { }
        return 0;
    }

    public int CompareTo(object? obj) {
        if (obj is ServiceDescriptorWrapper other) {
            return CompareTo(other);
        }
        throw new ArgumentException("Object is not a ServiceDescriptorWrapper", nameof(obj));
    }
}

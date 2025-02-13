namespace Benjft.Util.DependencyInjection.Exceptions;

public class FactoryMethodNotStaticException : InvalidFactoryMethodException {
    public FactoryMethodNotStaticException() { }
    public FactoryMethodNotStaticException(string? message) 
        : base(message) { }
    public FactoryMethodNotStaticException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

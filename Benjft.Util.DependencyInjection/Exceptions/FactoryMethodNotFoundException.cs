namespace Benjft.Util.DependencyInjection.Exceptions;

public class FactoryMethodNotFoundException : InvalidFactoryMethodException {
    public FactoryMethodNotFoundException() { }
    public FactoryMethodNotFoundException(string? message) 
        : base(message) { }
    public FactoryMethodNotFoundException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

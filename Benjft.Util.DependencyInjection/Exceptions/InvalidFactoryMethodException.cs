namespace Benjft.Util.DependencyInjection.Exceptions;

public class InvalidFactoryMethodException : DependencyInjectionAttributeException {
    public InvalidFactoryMethodException() { }
    public InvalidFactoryMethodException(string? message) 
        : base(message) { }
    public InvalidFactoryMethodException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

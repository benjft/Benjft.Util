namespace Benjft.Util.DependencyInjection.Exceptions;

public class InvalidServiceTypeException : DependencyInjectionAttributeException {
    public InvalidServiceTypeException() { }
    public InvalidServiceTypeException(string? message) 
        : base(message) { }
    public InvalidServiceTypeException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

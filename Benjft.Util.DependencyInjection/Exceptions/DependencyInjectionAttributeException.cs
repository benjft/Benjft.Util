namespace Benjft.Util.DependencyInjection.Exceptions;

public abstract class DependencyInjectionAttributeException : Exception {
    protected DependencyInjectionAttributeException() { }
    protected DependencyInjectionAttributeException(string? message) 
        : base(message) { }
    protected DependencyInjectionAttributeException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

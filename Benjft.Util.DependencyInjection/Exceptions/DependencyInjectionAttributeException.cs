namespace Benjft.Util.DependencyInjection.Exceptions;

public abstract class DependencyInjectionAttributeException : Exception {
    internal DependencyInjectionAttributeException(string? message) 
        : base(message) { }
    internal DependencyInjectionAttributeException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

namespace Benjft.Util.DependencyInjection.Exceptions;

public class FactoryMethodHasWrongSignatureException : InvalidFactoryMethodException {
    public FactoryMethodHasWrongSignatureException() { }
    public FactoryMethodHasWrongSignatureException(string? message) 
        : base(message) { }
    public FactoryMethodHasWrongSignatureException(string? message, Exception? innerException) 
        : base(message, innerException) { }
}

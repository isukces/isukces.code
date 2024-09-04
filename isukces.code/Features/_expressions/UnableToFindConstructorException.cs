#nullable enable
using System;

namespace iSukces.Code.AutoCode
{
    public class UnableToFindConstructorException : Exception
    {
        public UnableToFindConstructorException(Type type, string message, Exception? innerException = null)
            : base($"Unable to find constructor for type {type}. {message}", innerException) =>
            Type = type;

        public Type Type { get; }
    }
}
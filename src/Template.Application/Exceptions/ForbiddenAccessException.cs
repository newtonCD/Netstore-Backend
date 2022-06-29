using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Template.Application.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
    {
    }

    public ForbiddenAccessException(string message)
        : base(message)
    {
    }

    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ForbiddenAccessException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}

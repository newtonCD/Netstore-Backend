using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Netstore.Core.Application.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class ForbiddenAccessException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class.
    /// </summary>
    public ForbiddenAccessException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="inner">The inner.</param>
    public ForbiddenAccessException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected ForbiddenAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

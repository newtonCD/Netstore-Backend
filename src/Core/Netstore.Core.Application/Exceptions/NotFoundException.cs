using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Netstore.Core.Application.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    public NotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="inner">The inner.</param>
    public NotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="key">The key.</param>
    public NotFoundException(string name, object key)
        : base($"Entidade \"{name}\" ({key}) não encontrada.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="args">The arguments.</param>
    public NotFoundException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
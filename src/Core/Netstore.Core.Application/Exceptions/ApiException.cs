using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Netstore.Core.Application.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class ApiException : Exception
{
    public ApiException()
        : base()
    {
    }

    public ApiException(string message)
        : base(message)
    {
    }

    public ApiException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    protected ApiException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
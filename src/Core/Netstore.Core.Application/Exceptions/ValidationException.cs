﻿using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace Netstore.Core.Application.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    protected ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public IDictionary<string, string[]> Errors { get; }
}
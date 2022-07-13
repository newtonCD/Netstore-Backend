using System;
using System.Collections.Generic;

namespace Netstore.Common.Results;

/// <summary>
/// Generic result wrapper. See <see cref="Result"/>, see <see cref="IResult{T}"/>
/// </summary>
/// <typeparam name="T">Parameter type</typeparam>
public class Result<T> : Result, IResult<T>
{
    public Result()
        : base()
    {
    }

    public T Data { get; set; }
}
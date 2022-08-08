#nullable enable

using System;

namespace Ardalis.GuardClauses;

public static class GuardClauseExtensions
{
    public static bool IsFalse(this IGuardClause guardClause, bool input, string? parameterName = null, string? message = null)
    {
        if (!input)
            throw new ArgumentException(message ?? $"Required input {parameterName} must be false.", parameterName);

        return input;
    }

    public static bool IsTrue(this IGuardClause guardClause, bool input, string? parameterName = null, string? message = null)
    {
        if (input)
            throw new ArgumentException(message ?? $"Required input {parameterName} must be true.", parameterName);

        return input;
    }
}
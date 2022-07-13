using Microsoft.AspNetCore.Components;
using System;
using System.Text;

namespace Netstore.Common.Extensions;

public static class StringExtensions
{
    public static T ToEnum<T>(this string value)
        where T : struct
    {
        if (!Enum.TryParse<T>(value, out var enumeration))
        {
            return default;
        }

        return enumeration;
    }

    public static MarkupString ToMarkupString(this string value)
    {
        return new MarkupString(value);
    }

    public static string ToInitials(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        string[] words = value.Split(" ");
        foreach (string word in words)
        {
            builder.Append(word, 0, 1);
        }

        return builder.ToString().ToUpper();
    }

    public static string TrimStart(this string target, string trimString)
    {
        if (string.IsNullOrEmpty(trimString)) return target;

        string result = target;
        while (result.StartsWith(trimString))
        {
            result = result[trimString.Length..];
        }

        return result;
    }
}
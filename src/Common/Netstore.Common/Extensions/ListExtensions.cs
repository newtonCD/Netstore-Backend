using System;
using System.Collections.Generic;
using System.Linq;

namespace Netstore.Common.Extensions;

public static class ListExtensions
{
    public static T ToEnumFlags<T>(this List<string> list)
        where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException(typeof(T).Name + " is not an Enum");
        T flags;
        list.RemoveAll(c => !Enum.TryParse(c, true, out flags));
        string commaSeparatedFlags = string.Join(",", list);
        Enum.TryParse(commaSeparatedFlags, true, out flags);
        return flags;
    }

    public static int Replace<T>(this IList<T> source, T oldValue, T newValue)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        int index = source.IndexOf(oldValue);
        if (index != -1)
            source[index] = newValue;
        return index;
    }

    public static List<T> NullIfEmpty<T>(this List<T> item)
    {
        return item.Count > 0 ? item : null;
    }

    /// <summary>
    /// Divides a list to sublists of specified length
    /// </summary>
    /// <param name="source">The original list</param>
    /// <param name="chunkSize">The length of sublists</param>
    /// <returns></returns>
    public static IEnumerable<List<T>> Chunk<T>(this List<T> source, int chunkSize)
    {
        for (int i = 0; i < source.Count; i += chunkSize)
        {
            yield return source.GetRange(i, Math.Min(chunkSize, source.Count - i));
        }
    }
}
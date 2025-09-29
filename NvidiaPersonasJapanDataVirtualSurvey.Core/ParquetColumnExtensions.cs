using System;
using System.Collections;
using System.Collections.Generic;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core;

internal static class ParquetColumnExtensions
{
    public static string? GetString(this IDictionary<string, Array> columns, string key, int rowIndex)
    {
        var value = columns[key].GetValue(rowIndex);
        return value switch
        {
            null => null,
            string s => s,
            _ => value.ToString()
        };
    }

    public static IReadOnlyList<string>? GetStringList(this IDictionary<string, Array> columns, string key, int rowIndex)
    {
        var value = columns[key].GetValue(rowIndex);
        if (value is null)
        {
            return null;
        }

        if (value is string s)
        {
            return string.IsNullOrWhiteSpace(s) ? null : new[] { s };
        }

        if (value is IEnumerable enumerable and not string)
        {
            var list = new List<string>();
            foreach (var item in enumerable)
            {
                if (item is null) continue;
                var text = item.ToString();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    list.Add(text);
                }
            }

            return list.Count > 0 ? list : null;
        }

        var fallback = value.ToString();
        return string.IsNullOrWhiteSpace(fallback) ? null : new[] { fallback! };
    }

    public static int? GetNullableInt(this IDictionary<string, Array> columns, string key, int rowIndex)
    {
        var value = columns[key].GetValue(rowIndex);
        return value switch
        {
            null => null,
            int i => i,
            long l => (int)l,
            short s => (int)s,
            byte b => (int)b,
            double d => (int)d,
            float f => (int)f,
            string s when int.TryParse(s, out var parsed) => parsed,
            _ => null
        };
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TableOfRecords;

/// <summary>

/// </summary>
public static class TableOfRecordsCreator
{
    private const char CornerChar = '+';
    private const char HorizontalChar = '-';
    private const char VerticalChar = '|';
    private const int CellPadding = 2;

    public static void WriteTable<T>(ICollection<T>? collection, TextWriter? writer)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(writer);

        if (collection.Count == 0)
        {
            throw new ArgumentException("Collection cannot be empty.", nameof(collection));
        }

        PropertyInfo[] properties = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead)
            .Where(p => p.GetIndexParameters().Length == 0) // excl
            .OrderBy(p => p.MetadataToken)
            .ToArray();

        if (properties.Length == 0)
        {
            throw new ArgumentException(
                $"Type {typeof(T).Name} has no readable public instance properties.",
                nameof(collection));
        }

        int colCount = properties.Length;

        bool[] rightAlign = new bool[colCount];
        for (int i = 0; i < colCount; i++)
        {
            Type t = Nullable.GetUnderlyingType(properties[i].PropertyType) ?? properties[i].PropertyType;
            rightAlign[i] = IsRightAlignedByType(t);
        }

        int[] widths = properties.Select(p => p.Name.Length).ToArray();

        var rows = new List<string[]>(collection.Count);

        foreach (var item in collection)
        {
            var row = new string[colCount];

            for (int i = 0; i < colCount; i++)
            {
                object? raw = properties[i].GetValue(item);
                string text = raw is null
                    ? string.Empty
                    : Convert.ToString(raw, CultureInfo.InvariantCulture) ?? string.Empty;

                row[i] = text;

                if (text.Length > widths[i])
                {
                    widths[i] = text.Length;
                }
            }

            rows.Add(row);
        }

        WriteBorder(writer, widths);

        WriteRow(
            writer,
            properties.Select(p => p.Name).ToArray(),
            widths,
            rightAlign);

        WriteBorder(writer, widths);

        foreach (var row in rows)
        {
            WriteRow(writer, row, widths, rightAlign);
            WriteBorder(writer, widths);
        }

        writer.Flush();
    }

    private static void WriteBorder(TextWriter writer, int[] widths)
    {
        writer.Write(CornerChar);

        for (int i = 0; i < widths.Length; i++)
        {
            writer.Write(new string(HorizontalChar, widths[i] + CellPadding));
            writer.Write(CornerChar);
        }

        writer.Write(Environment.NewLine);
    }

    private static void WriteRow(TextWriter writer, string[] values, int[] widths, bool[] rightAlign)
    {
        var sb = new StringBuilder();
        sb.Append(VerticalChar);

        for (int i = 0; i < widths.Length; i++)
        {
            string value = values[i] ?? string.Empty;

            string cell = rightAlign[i]
                ? value.PadLeft(widths[i])
                : value.PadRight(widths[i]);

            _ = sb.Append(' ')
              .Append(cell)
              .Append(' ')
              .Append(VerticalChar);
        }

        sb.Append(value: Environment.NewLine);
        writer.Write(sb.ToString());
    }

    private static bool IsRightAlignedByType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(decimal))
        {
            return true;
        }

        if (type == typeof(string) || type == typeof(char) || type == typeof(bool) || type.IsEnum)
        {
            return false;
        }

        if (type == typeof(DateTime) || type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan) || type == typeof(DateOnly) || type == typeof(TimeOnly))
        {
            return true;
        }

        if (type == typeof(byte) || type == typeof(sbyte) ||
            type == typeof(short) || type == typeof(ushort) ||
            type == typeof(int) || type == typeof(uint) ||
            type == typeof(long) || type == typeof(ulong) ||
            type == typeof(float) || type == typeof(double))
        {
            return true;
        }

        return false;
    }
}

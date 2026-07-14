using System;
using System.Globalization;

namespace ValidationAttributes;

public sealed class StringLengthAttribute : ValidationAttribute
{
    public StringLengthAttribute(int minimumLength, int maximumLength)
        : base("{0} length must be between {1} and {2}.")
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minimumLength);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumLength);

        if (minimumLength > maximumLength)
        {
            throw new ArgumentException("Minimum length must be less than or equal to maximum length.");
        }

        this.MinimumLength = minimumLength;
        this.MaximumLength = maximumLength;
    }

    public int MinimumLength { get; }

    public int MaximumLength { get; }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // required kontrolünü ValidationService yapacak
        }

        if (value is not string s)
        {
            return false;
        }

        int len = s.Length;
        return len >= this.MinimumLength && len <= this.MaximumLength;
    }

    public override string FormatErrorMessage(string name)
    {
        var format = this.ErrorMessage ?? "{0} length must be between {1} and {2}.";

        return string.Format(
            CultureInfo.InvariantCulture,
            format,
            name,
            this.MinimumLength,
            this.MaximumLength);
    }
}

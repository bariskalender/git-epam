using System;
using System.Globalization;

namespace ValidationAttributes;

public sealed class RequiredAttribute : ValidationAttribute
{
    public RequiredAttribute()
        : base("{0} is required.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        if (value is string s)
        {
            return s.Length > 0;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.InvariantCulture, this.ErrorMessage!, name);
}

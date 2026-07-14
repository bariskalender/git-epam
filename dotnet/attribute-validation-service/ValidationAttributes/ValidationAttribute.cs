using System;
using System.Globalization;

namespace ValidationAttributes;

[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = true,
    Inherited = true)]
public abstract class ValidationAttribute : Attribute
{
    protected ValidationAttribute()
        : this(null)
    {
    }

    protected ValidationAttribute(string? defaultErrorMessage)
    {
        this.ErrorMessage = string.IsNullOrWhiteSpace(defaultErrorMessage)
            ? "The field is invalid."
            : defaultErrorMessage;
    }

    public string? ErrorMessage { get; set; }

    public abstract bool IsValid(object? value);

    public virtual string FormatErrorMessage(string name)
        => string.Format(CultureInfo.InvariantCulture, this.ErrorMessage ?? "The field is invalid.", name);
}

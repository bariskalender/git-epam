using System;
using System.Globalization;

namespace ValidationAttributes;

public sealed class NumericRangeAttribute : ValidationAttribute
{
    private readonly object minimum;
    private readonly object maximum;

    public NumericRangeAttribute(double minimum, double maximum)
        : this(minimum, maximum, typeof(double))
    {
    }

    // Testler bunu kullanıyor (object min/max + Type)
    public NumericRangeAttribute(object minimum, object maximum, Type operandType)
        : base("{0} must be between {1} and {2}.")
    {
        this.minimum = minimum ?? throw new ArgumentNullException(nameof(minimum));
        this.maximum = maximum ?? throw new ArgumentNullException(nameof(maximum));
        _ = operandType ?? throw new ArgumentNullException(nameof(operandType)); // sadece null kontrolü
    }

    public object Minimum => this.minimum;

    public object Maximum => this.maximum;

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (!TryToDecimal(this.minimum, out decimal minDec) ||
            !TryToDecimal(this.maximum, out decimal maxDec))
        {
            return false;
        }

        // IMPORTANT: ctor’da değil burada throw (test beklentisi)
        if (minDec >= maxDec)
        {
            throw new ArgumentException("Minimum must be less than maximum.");
        }

        if (!TryToDecimal(value, out decimal valDec))
        {
            return false;
        }

        return valDec >= minDec && valDec <= maxDec;
    }

    public override string FormatErrorMessage(string name)
    {
        var format = this.ErrorMessage ?? "{0} must be between {1} and {2}.";

        return string.Format(
            CultureInfo.InvariantCulture,
            format,
            name,
            this.minimum,
            this.maximum);
    }

    private static bool TryToDecimal(object value, out decimal result)
    {
        try
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    result = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    return true;

                default:
                    result = default;
                    return false;
            }
        }
        catch (FormatException)
        {
            result = default;
            return false;
        }
        catch (InvalidCastException)
        {
            result = default;
            return false;
        }
        catch (OverflowException)
        {
            result = default;
            return false;
        }
    }
}

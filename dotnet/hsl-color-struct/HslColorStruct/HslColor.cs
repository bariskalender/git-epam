using System;

namespace HslColorStruct;

/// <summary>
/// Represents a color in the HSL color model.
/// </summary>
public readonly struct HslColor : IEquatable<HslColor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HslColor"/> struct with the specified <paramref name="hue"/>, <paramref name="saturation"/> and <paramref name="lightness"/>.
    /// </summary>
    /// <param name="hue">The hue component (0-360 degrees).</param>
    /// <param name="saturation">The saturation component (0-100%).</param>
    /// <param name="lightness">The lightness component (0-100%).</param>
    /// <exception cref="ArgumentException">Thrown when any of the components is outside its valid range.</exception>
    public HslColor(int hue, int saturation, int lightness)
    {
        ThrowExceptionIfValueIsNotValid(hue, nameof(hue), 0, 360);
        ThrowExceptionIfValueIsNotValid(saturation, nameof(saturation), 0, 100);
        ThrowExceptionIfValueIsNotValid(lightness, nameof(lightness), 0, 100);

        this.Hue = hue;
        this.Saturation = saturation;
        this.Lightness = lightness;
    }

    /// <summary>
    /// Gets the hue component (0-360 degrees).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when value is outside the valid range (0-360).</exception>
    public int Hue { get; init; }

    /// <summary>
    /// Gets the saturation component (0-100%).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when value is outside the valid range (0-100).</exception>
    public int Saturation { get; init; }

    /// <summary>
    /// Gets the lightness component (0-100%).
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when value is outside the valid range (0-100).</exception>
    public int Lightness { get; init; }

    /// <summary>
    /// Creates a new instance of the <see cref="HslColor"/> struct with specified <paramref name="hue"/>, <paramref name="saturation"/> and <paramref name="lightness"/>.
    /// </summary>
    /// <param name="hue">The hue component (0-360 degrees).</param>
    /// <param name="saturation">The saturation component (0-100%).</param>
    /// <param name="lightness">The lightness component (0-100%).</param>
    /// <returns>A <see cref="HslColor"/> that is initialized with specified components.</returns>
    /// <exception cref="ArgumentException">Thrown when any of the components is outside its valid range.</exception>
    public static HslColor Create(int hue, int saturation, int lightness)
    {
        return new HslColor(hue, saturation, lightness);
    }

    /// <summary>
    /// Converts the string representation of a color to its <see cref="HslColor"/> equivalent.
    /// </summary>
    /// <param name="hslString">A string containing a color to convert in format "H,S,L".</param>
    /// <returns>An <see cref="HslColor"/> equivalent to the color contained in <paramref name="hslString"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="hslString"/> is invalid.</exception>
    public static HslColor Parse(string hslString)
    {
        if (!TryParse(hslString, out var color))
        {
            throw new ArgumentException(
                "Invalid HSL color string. Expected format: \"H,S,L\" with integer values.",
                nameof(hslString));
        }

        return color;
    }

    /// <summary>
    /// Converts the string representation of a color to its <see cref="HslColor"/> equivalent. A return value indicates whether the operation succeeded.
    /// </summary>
    /// <param name="hslString">A string containing a color to convert in format "H,S,L".</param>
    /// <param name="hslColor">A <see cref="HslColor"/> equivalent to the color contained in <paramref name="hslString"/>.</param>
    /// <returns>true if <paramref name="hslString"/> was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string hslString, out HslColor hslColor)
    {
        hslColor = default;

        if (string.IsNullOrWhiteSpace(hslString))
        {
            return false;
        }

        // Strict format: no leading/trailing whitespace
        if (hslString != hslString.Trim())
        {
            return false;
        }

        var parts = hslString.Split(',');

        if (parts.Length != 3)
        {
            return false;
        }

        // Strict format: no whitespace inside components
        if (parts[0].Contains(' ', StringComparison.Ordinal) ||
            parts[1].Contains(' ', StringComparison.Ordinal) ||
            parts[2].Contains(' ', StringComparison.Ordinal))
        {
            return false;
        }

        if (!int.TryParse(parts[0], out var h))
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var s))
        {
            return false;
        }

        if (!int.TryParse(parts[2], out var l))
        {
            return false;
        }

        try
        {
            hslColor = new HslColor(h, s, l);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the specified <see cref="HslColor"/> is equal to the current <see cref="HslColor"/>.
    /// </summary>
    public bool Equals(HslColor other)
    {
        return this.Hue == other.Hue && this.Saturation == other.Saturation && this.Lightness == other.Lightness;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="HslColor"/>.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is HslColor other && this.Equals(other);
    }

    /// <summary>
    /// Gets a hash code of the current <see cref="HslColor"/>.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Hue, this.Saturation, this.Lightness);
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="HslColor"/>.
    /// </summary>
    public override string ToString()
    {
        return $"{this.Hue},{this.Saturation},{this.Lightness}";
    }

    public static bool operator ==(HslColor left, HslColor right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(HslColor left, HslColor right)
    {
        return !left.Equals(right);
    }

    private static void ThrowExceptionIfValueIsNotValid(int value, string parameterName, int minValue, int maxValue)
    {
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentException(
                $"Parameter '{parameterName}' must be in range [{minValue}..{maxValue}].",
                parameterName);
        }
    }
}

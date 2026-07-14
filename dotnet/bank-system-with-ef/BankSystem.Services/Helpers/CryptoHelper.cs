using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace BankSystem.Services.Helpers;

/// <summary>
/// Provides helper methods for cryptographic operations.
/// </summary>
internal static class CryptoHelper
{
    /// <summary>
    /// Generates a string that represents SHA-256 hash of the given input.
    /// </summary>
    /// <param name="input">The string to generate a hash for.</param>
    /// <returns>A string that represents a 128-bit long SHA-256 hash value of the input string.</returns>
    /// <remarks>
    /// Using SHA-256 for hashing, it creates a hash and then takes first 16 bytes, reducing it to a 128-bit hash.
    /// The method will return the hash value as a string of 32 alphanumeric characters (16 bytes).
    /// If implemented in security-concerned contexts, use a complete hash to maintain collision resistance.
    /// Please be aware this method does not provide a means to validate the existence of the given string.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when a null or empty string is passed.</exception>
    public static string GenerateHash(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var hashStringBuilder = new StringBuilder(32);

        for (var i = 0; i < 16; i++)
        {
            _ = hashStringBuilder.Append(hashBytes[i].ToString("x2", CultureInfo.InvariantCulture));
        }

        return hashStringBuilder.ToString();
    }
}

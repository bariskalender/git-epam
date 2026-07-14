using System;
using Validation;

namespace UriConversion
{
    public class UriValidator : IValidator<string>
    {
        public bool IsValid(string obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (string.IsNullOrWhiteSpace(obj))
            {
                return false;
            }

            if (!Uri.TryCreate(obj, UriKind.Absolute, out var uri))
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(uri.Scheme)
                && !string.IsNullOrWhiteSpace(uri.Host)
                && !string.IsNullOrWhiteSpace(uri.AbsolutePath)
                && uri.AbsolutePath != "/";
        }
    }
}

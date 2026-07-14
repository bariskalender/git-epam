using System;
using Conversion;
using Validation;

namespace UriConversion
{
    public class UriConverter : IConverter<Uri?>
    {
        private readonly IValidator<string> validator;

        public UriConverter(IValidator<string> validator)
        {
            this.validator = validator;
        }

        public Uri? Convert(string? obj)
        {
            if (obj is null)
            {
                return null;
            }

            if (!this.validator.IsValid(obj))
            {
                return null;
            }

            return new Uri(obj);
        }
    }
}

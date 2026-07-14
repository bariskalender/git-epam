namespace ContiguousSubstrings
{
    public static class Sequences
    {
        public static IEnumerable<string> GetSubstrings(string numbers, int length)
        {
            if (string.IsNullOrWhiteSpace(numbers))
            {
                throw new ArgumentException(
                    "Input string cannot be null, empty, or whitespace.",
                    nameof(numbers));
            }

            if (length <= 0)
            {
                throw new ArgumentException(
                    "Length must be greater than zero.",
                    nameof(length));
            }

            if (length > numbers.Length)
            {
                throw new ArgumentException(
                    "Length cannot be greater than the input string length.",
                    nameof(length));
            }

            foreach (char c in numbers)
            {
                if (!char.IsDigit(c))
                {
                    throw new ArgumentException(
                        "Input string must contain only digits (0-9).",
                        nameof(numbers));
                }
            }

            for (int i = 0; i <= numbers.Length - length; i++)
            {
                yield return numbers.Substring(i, length);
            }
        }
    }
}

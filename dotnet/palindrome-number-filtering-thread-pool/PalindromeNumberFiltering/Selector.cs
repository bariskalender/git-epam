using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace PalindromeNumberFiltering
{
    public static class Selector
    {
        public static IList<int> GetPalindromes(IList<int> numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            if (numbers.Count == 0)
            {
                return Array.Empty<int>();
            }

            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            using CountdownEvent countdown = new CountdownEvent(numbers.Count);

            for (int i = 0; i < numbers.Count; i++)
            {
                int n = numbers[i];

                ThreadPool.QueueUserWorkItem(
                    static state =>
                    {
                        WorkItemPayload payload = (WorkItemPayload)state!;

                        try
                        {
                            if (IsPalindrome(payload.Number))
                            {
                                payload.Bag.Add(payload.Number);
                            }
                        }
                        finally
                        {
                            payload.Countdown.Signal();
                        }
                    },
                    new WorkItemPayload(n, bag, countdown));
            }

            countdown.Wait();
            return bag.ToArray();
        }

        private static bool IsPalindrome(int number)
        {
            if (number < 0)
            {
                return false;
            }

            byte length = GetLength(number);
            return IsPositiveNumberPalindrome(number, 0, length - 1);
        }

        private static bool IsPositiveNumberPalindrome(int number, int left, int right)
        {
            if (left >= right)
            {
                return true;
            }

            int leftDigit = GetDigitInDecimalPlace(number, right);
            int rightDigit = GetDigitInDecimalPlace(number, left);

            if (leftDigit != rightDigit)
            {
                return false;
            }

            return IsPositiveNumberPalindrome(number, left + 1, right - 1);
        }

        private static int GetDigitInDecimalPlace(int number, int decimalPlace)
        {
            int divisor = 1;

            for (int i = 0; i < decimalPlace; i++)
            {
                divisor *= 10;
            }

            return (number / divisor) % 10;
        }

        private static byte GetLength(int number)
        {
            if (number < 0)
            {
                number = -number;
            }

            return number switch
            {
                < 10 => 1,
                < 100 => 2,
                < 1_000 => 3,
                < 10_000 => 4,
                < 100_000 => 5,
                < 1_000_000 => 6,
                < 10_000_000 => 7,
                < 100_000_000 => 8,
                < 1_000_000_000 => 9,
                _ => 10,
            };
        }

        private sealed record WorkItemPayload(int Number, ConcurrentBag<int> Bag, CountdownEvent Countdown);
    }
}

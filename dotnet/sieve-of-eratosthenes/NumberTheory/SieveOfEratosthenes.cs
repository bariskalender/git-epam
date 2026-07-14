using System.Collections.Concurrent;
using System.Threading;

namespace NumberTheory;

public static class SieveOfEratosthenes
{
    public static IEnumerable<int> GetPrimeNumbersSequentialAlgorithm(int n)
    {
        ValidateInput(n);
        return SieveSequential(n);
    }

    public static IEnumerable<int> GetPrimeNumbersModifiedSequentialAlgorithm(int n)
    {
        ValidateInput(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        if (n == 2)
        {
            return new[] { 2 };
        }

        bool[] composite = new bool[n + 1];
        int limit = (int)Math.Sqrt(n);

        for (int candidate = 3; candidate <= limit; candidate += 2)
        {
            if (composite[candidate])
            {
                continue;
            }

            long start = (long)candidate * candidate;
            for (long multiple = start; multiple <= n; multiple += candidate * 2L)
            {
                composite[multiple] = true;
            }
        }

        List<int> primes = new() { 2 };

        for (int i = 3; i <= n; i += 2)
        {
            if (!composite[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    public static IEnumerable<int> GetPrimeNumbersConcurrentDataDecomposition(int n)
    {
        ValidateInput(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        int sqrt = (int)Math.Sqrt(n);
        int[] basePrimes = SieveSequential(sqrt).ToArray();

        int processorCount = Math.Max(1, Environment.ProcessorCount);
        int startNumber = 2;
        int totalNumbers = n - startNumber + 1;
        int chunkSize = Math.Max(1, (int)Math.Ceiling(totalNumbers / (double)processorCount));

        ConcurrentBag<List<int>> partialResults = new();

        Parallel.For(0, processorCount, workerIndex =>
        {
            int segmentStart = startNumber + (workerIndex * chunkSize);
            int segmentEnd = Math.Min(n, segmentStart + chunkSize - 1);

            if (segmentStart > n)
            {
                return;
            }

            bool[] composite = new bool[segmentEnd - segmentStart + 1];

            foreach (int prime in basePrimes)
            {
                long firstMultiple = Math.Max((long)prime * prime, GetFirstMultipleInSegment(segmentStart, prime));

                for (long multiple = firstMultiple; multiple <= segmentEnd; multiple += prime)
                {
                    composite[multiple - segmentStart] = true;
                }
            }

            List<int> localPrimes = new();

            for (int number = segmentStart; number <= segmentEnd; number++)
            {
                if (number >= 2 && !composite[number - segmentStart])
                {
                    localPrimes.Add(number);
                }
            }

            partialResults.Add(localPrimes);
        });

        return partialResults
            .SelectMany(x => x)
            .OrderBy(x => x)
            .ToArray();
    }

    public static IEnumerable<int> GetPrimeNumbersConcurrentBasicPrimesDecomposition(int n)
    {
        ValidateInput(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        bool[] composite = new bool[n + 1];
        int sqrt = (int)Math.Sqrt(n);
        int[] basePrimes = SieveSequential(sqrt).ToArray();

        Parallel.ForEach(basePrimes, prime =>
        {
            long start = (long)prime * prime;

            for (long multiple = start; multiple <= n; multiple += prime)
            {
                composite[multiple] = true;
            }
        });

        List<int> primes = new();

        for (int i = 2; i <= n; i++)
        {
            if (!composite[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    public static IEnumerable<int> GetPrimeNumbersConcurrentWithThreadPool(int n)
    {
        ValidateInput(n);

        if (n < 2)
        {
            return Array.Empty<int>();
        }

        int sqrt = (int)Math.Sqrt(n);
        int[] basePrimes = SieveSequential(sqrt).ToArray();

        int processorCount = Math.Max(1, Environment.ProcessorCount);
        int startNumber = 2;
        int totalNumbers = n - startNumber + 1;
        int workerCount = Math.Min(processorCount, totalNumbers);
        int chunkSize = Math.Max(1, (int)Math.Ceiling(totalNumbers / (double)workerCount));

        ConcurrentBag<List<int>> partialResults = new();
        using CountdownEvent countdown = new(workerCount);

        for (int workerIndex = 0; workerIndex < workerCount; workerIndex++)
        {
            int segmentStart = startNumber + (workerIndex * chunkSize);
            int segmentEnd = Math.Min(n, segmentStart + chunkSize - 1);

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (segmentStart > n)
                    {
                        partialResults.Add(new List<int>());
                        return;
                    }

                    bool[] composite = new bool[segmentEnd - segmentStart + 1];

                    foreach (int prime in basePrimes)
                    {
                        long firstMultiple = Math.Max((long)prime * prime, GetFirstMultipleInSegment(segmentStart, prime));

                        for (long multiple = firstMultiple; multiple <= segmentEnd; multiple += prime)
                        {
                            composite[multiple - segmentStart] = true;
                        }
                    }

                    List<int> localPrimes = new();

                    for (int number = segmentStart; number <= segmentEnd; number++)
                    {
                        if (number >= 2 && !composite[number - segmentStart])
                        {
                            localPrimes.Add(number);
                        }
                    }

                    partialResults.Add(localPrimes);
                }
                finally
                {
                    countdown.Signal();
                }
            });
        }

        countdown.Wait();

        return partialResults
            .SelectMany(x => x)
            .OrderBy(x => x)
            .ToArray();
    }

    private static void ValidateInput(int n)
    {
        if (n <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "The upper limit must be greater than zero.");
        }
    }

    private static IEnumerable<int> SieveSequential(int n)
    {
        if (n < 2)
        {
            return Array.Empty<int>();
        }

        bool[] composite = new bool[n + 1];
        int limit = (int)Math.Sqrt(n);

        for (int p = 2; p <= limit; p++)
        {
            if (composite[p])
            {
                continue;
            }

            for (int multiple = p * p; multiple <= n; multiple += p)
            {
                composite[multiple] = true;
            }
        }

        List<int> primes = new();

        for (int i = 2; i <= n; i++)
        {
            if (!composite[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    private static long GetFirstMultipleInSegment(int segmentStart, int prime)
    {
        long remainder = segmentStart % prime;
        return remainder == 0 ? segmentStart : segmentStart + (prime - remainder);
    }
}

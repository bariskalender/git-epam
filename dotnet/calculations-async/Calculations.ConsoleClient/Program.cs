using System;
using System.Threading;
using System.Threading.Tasks;
using Calculations;

namespace Calculations.ConsoleClient
{
    internal static class Program
    {
        private static readonly object ConsoleLock = new object();

        /// <summary>
        /// Calculates the sum from 1 to n asynchronously.
        /// The value of n is set by the user from the console.
        /// The user can change the boundary n during the calculation, which causes the calculation to be restarted,
        /// this should not crash the application.
        /// After receiving the result, be able to continue calculations without leaving the console.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task Main()
        {
            CancellationTokenSource? currentCancellationTokenSource = null;
            int currentCalculationId = 0;

            WriteLineSafe("Async sum calculator");
            WriteLineSafe("Enter a positive integer to start calculation.");
            WriteLineSafe("Enter a new number while calculation is running to restart it.");
            WriteLineSafe("Type 'exit' to close the application.");
            WriteLineSafe(string.Empty);

            while (true)
            {
                WriteSafe("Enter n (or 'exit'): ");
                string? input = await Task.Run(Console.ReadLine);

                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    if (currentCancellationTokenSource is not null)
                    {
                        await currentCancellationTokenSource.CancelAsync();
                        currentCancellationTokenSource.Dispose();
                    }

                    return;
                }

                if (!int.TryParse(input, out int n))
                {
                    WriteLineSafe("Invalid input. Please enter a positive integer.");
                    continue;
                }

                if (n <= 0)
                {
                    WriteLineSafe("n must be greater than zero.");
                    continue;
                }

                if (currentCancellationTokenSource is not null)
                {
                    await currentCancellationTokenSource.CancelAsync();
                    currentCancellationTokenSource.Dispose();
                }

                currentCancellationTokenSource = new CancellationTokenSource();
                currentCalculationId++;
                int calculationId = currentCalculationId;
                CancellationToken token = currentCancellationTokenSource.Token;

                var progress = new SynchronousProgress<(int index, long sum)>(p =>
                {
                    if (calculationId != currentCalculationId)
                    {
                        return;
                    }

                    if (ShouldPrintProgress(p.index, n))
                    {
                        WriteLineSafe($"Progress: index = {p.index}, sum = {p.sum}");
                    }
                });

                WriteLineSafe($"Calculation started for n = {n}.");

                _ = RunCalculationAsync(
                    n,
                    calculationId,
                    progress,
                    () => calculationId == currentCalculationId,
                    token);
            }
        }

        private static async Task RunCalculationAsync(
            int n,
            int calculationId,
            IProgress<(int index, long sum)> progress,
            Func<bool> isCurrentCalculation,
            CancellationToken token)
        {
            try
            {
                long result = await Calculator.CalculateSumAsync(n, token, progress);

                if (isCurrentCalculation())
                {
                    WriteLineSafe($"Result for n = {n}: {result}");
                }
            }
            catch (OperationCanceledException)
            {
                if (isCurrentCalculation())
                {
                    WriteLineSafe($"Calculation for n = {n} was cancelled.");
                }
            }
            catch (Exception ex)
            {
                if (isCurrentCalculation())
                {
                    WriteLineSafe($"Calculation for n = {n} failed: {ex.Message}");
                }
            }
        }

        private static bool ShouldPrintProgress(int index, int n)
        {
            if (n <= 100)
            {
                return true;
            }

            int step = Math.Max(1, n / 10);
            return index % step == 0 || index == n;
        }

        private static void WriteSafe(string message)
        {
            lock (ConsoleLock)
            {
                Console.Write(message);
            }
        }

        private static void WriteLineSafe(string message)
        {
            lock (ConsoleLock)
            {
                Console.WriteLine(message);
            }
        }

        private sealed class SynchronousProgress<T> : IProgress<T>
        {
            private readonly Action<T> handler;

            public SynchronousProgress(Action<T> handler)
            {
                this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
            }

            public void Report(T value)
            {
                this.handler(value);
            }
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tpl;

public static class TaskStatusHandler
{
    public static Task CreateTaskWithCreatedStatus()
    {
        return new Task(() => { });
    }

    public static Task CreateTaskWithWaitingForActivationStatus()
    {
        var taskCompletionSource = new TaskCompletionSource<object?>();
        return taskCompletionSource.Task;
    }

    public static Task CreateTaskWithWaitingToRunStatus()
    {
        var schedulerPair = new ConcurrentExclusiveSchedulerPair(
            TaskScheduler.Default,
            maxConcurrencyLevel: 1);

        var blockerStarted = 0;
        var releaseBlocker = 0;

        Task.Factory.StartNew(
            () =>
            {
                Interlocked.Exchange(ref blockerStarted, 1);

                while (Volatile.Read(ref releaseBlocker) == 0)
                {
                    Thread.SpinWait(1000);
                }
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            schedulerPair.ExclusiveScheduler);

        SpinWait.SpinUntil(() => Volatile.Read(ref blockerStarted) == 1, 1000);

        var waitingTask = Task.Factory.StartNew(
            () => { },
            CancellationToken.None,
            TaskCreationOptions.None,
            schedulerPair.ExclusiveScheduler);

        SpinWait.SpinUntil(() => waitingTask.Status == TaskStatus.WaitingToRun, 1000);

        return waitingTask;
    }

    public static Task CreateTaskWithRunningStatus()
    {
        var started = 0;
        var release = 0;

        var task = Task.Factory.StartNew(
            () =>
            {
                Interlocked.Exchange(ref started, 1);

                while (Volatile.Read(ref release) == 0)
                {
                    Thread.SpinWait(1000);
                }
            },
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);

        SpinWait.SpinUntil(() => Volatile.Read(ref started) == 1, 1000);

        return task;
    }

    public static Task CreateTaskWithRanToCompletionStatus()
    {
        var task = Task.Run(() => { });
        task.Wait();

        return task;
    }

    public static Task CreateTaskWithWaitingForChildrenToCompleteStatus()
    {
        var childStarted = 0;
        var releaseChild = 0;

        var parentTask = Task.Factory.StartNew(() =>
        {
            Task.Factory.StartNew(
                () =>
                {
                    Interlocked.Exchange(ref childStarted, 1);

                    while (Volatile.Read(ref releaseChild) == 0)
                    {
                        Thread.SpinWait(1000);
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.AttachedToParent,
                TaskScheduler.Current);
        });

        SpinWait.SpinUntil(() => Volatile.Read(ref childStarted) == 1, 1000);
        SpinWait.SpinUntil(
            () => parentTask.Status == TaskStatus.WaitingForChildrenToComplete,
            1000);

        return parentTask;
    }

    public static Task CreateTaskWithIsCompletedStatus()
    {
        return Task.CompletedTask;
    }

    public static Task CreateTaskWithIsCancelledStatus()
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        var task = Task.Run(
            () =>
            {
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
            },
            cancellationTokenSource.Token);

        cancellationTokenSource.Cancel();

        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
            // Expected for a canceled task.
        }

        return task;
    }

    public static Task CreateTaskWithIsFaultedStatus()
    {
        var task = Task.Run(() => throw new InvalidOperationException("Task failed."));

        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
            // Expected for a faulted task.
        }

        return task;
    }
}

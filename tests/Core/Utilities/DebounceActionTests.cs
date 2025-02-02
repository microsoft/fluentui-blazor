// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.FluentUI.AspNetCore.Components.Utilities.InternalDebounce;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

#pragma warning disable CS0618 // Type or member is obsolete

[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Tests purpose")]
public class DebounceActionTests
{
    private readonly ITestOutputHelper Output;

    public DebounceActionTests(ITestOutputHelper output)
    {
        Output = output;
        Debounce = new DebounceAction();
    }

    private DebounceAction Debounce { get; init; }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_Default()
    {
        // Arrange
        var actionCalled = false;
        var watcher = Stopwatch.StartNew();

        // Act
        Debounce.Run(50, async () =>
        {
            actionCalled = true;
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await Debounce.CurrentTask;

        // Assert
        Assert.True(watcher.ElapsedMilliseconds >= 50);
        Assert.True(actionCalled);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_MultipleCalls()
    {
        // Arrange
        var actionCalledCount = 0;
        var actionCalled = string.Empty;

        // Act
        Debounce.Run(50, async () =>
        {
            actionCalled = "Step1";
            actionCalledCount++;
            await Task.CompletedTask;
        });

        Debounce.Run(40, async () =>
        {
            actionCalled = "Step2";
            actionCalledCount++;
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await Debounce.CurrentTask;

        // Assert
        Assert.Equal("Step2", actionCalled);
        Assert.Equal(1, actionCalledCount);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_MultipleCalls_Async()
    {
        var watcher = Stopwatch.StartNew();

        // Arrange
        var step1Started = false;
        var actionCalledCount = 0;
        var actionCalled = string.Empty;
        var actionNextCount = 0;
        var actionNextCalled = string.Empty;

        // Act: simulate two async calls
        var t1 = Task.Run(async () =>
        {
            step1Started = true;
            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Start1");
            try
            {
                await Debounce.RunAsync(50, async () =>
                {
                    await Task.Delay(1000); // Let time for the second task to start, and to cancel this one

                    Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step1");
                    actionCalled = "Step1";
                    actionCalledCount++;
                    await Task.CompletedTask;
                });

                Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: CurrentTask: IsFaulted={Debounce.CurrentTask.IsFaulted} IsCanceled={Debounce.CurrentTask.IsCanceled} IsCompleted={Debounce.CurrentTask.IsCompleted} IsCompletedSuccessfully={Debounce.CurrentTask.IsCompletedSuccessfully}");
                if (Debounce.CurrentTask.IsCanceled || Debounce.CurrentTask.IsFaulted)
                {
                    Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: CurrentTask Canceled");
                    return;
                }

                Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Next1");
                actionNextCalled = "Next1";
                actionNextCount++;
            }
            catch (TaskCanceledException)
            {
                Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Task1 TaskCanceled");
            }
            catch (OperationCanceledException)
            {
                Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Task1 OperationCanceled");
            }
        });

        // Wait for Step1 to start.
        while (!step1Started)
        {
            await Task.Delay(10);
        }

        var t2 = Task.Run(async () =>
        {
            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Start2");
            await Debounce.RunAsync(40, async () =>
            {
                Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step2");
                actionCalled = "Step2";
                actionCalledCount++;
                await Task.CompletedTask;
            });

            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Next2");
            actionNextCalled = "Next2";
            actionNextCount++;
        });

        await Task.WhenAll(t1, t2);

        // Assert
        Assert.Equal("Step2", actionCalled);
        Assert.Equal(1, actionCalledCount);

        Assert.Equal("Next2", actionNextCalled);
        Assert.Equal(1, actionNextCount);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_Disposed()
    {
        // Arrange
        var actionCalled = false;

        // Act
        Debounce.Dispose();

        Debounce.Run(50, async () =>
        {
            actionCalled = true;
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await Debounce.CurrentTask;

        // Assert
        Assert.False(actionCalled);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_Busy()
    {
        // Act
        Debounce.Run(50, async () =>
        {
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await Debounce.CurrentTask;

        // Assert
        Assert.False(Debounce.Busy);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_Exception()
    {
        await Assert.ThrowsAsync<AggregateException>(async () =>
        {
            // Act
            Debounce.Run(50, async () =>
            {
                await Task.CompletedTask;
                throw new InvalidProgramException("Error");     // Simulate an exception
            });

            // Wait for the debounce to complete
            await Debounce.CurrentTask;
        });

        // Assert
        Assert.False(Debounce.Busy);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_AsyncException()
    {
        await Assert.ThrowsAsync<AggregateException>(async () =>
        {
            // Act
            await Debounce.RunAsync(50, async () =>
            {
                await Task.CompletedTask;
                throw new InvalidProgramException("Error");     // Simulate an exception
            });
        });

        // Assert
        Assert.False(Debounce.Busy);
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public void Debounce_DelayMustBePositive()
    {
        // Act
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Debounce.Run(-10, () => Task.CompletedTask);
        });
    }

    [Fact(Skip = "Locally validated, but some CI/CD failures are due to poor server performance.")]
    public async Task Debounce_FirstRunAlreadyStarted()
    {
        var watcher = Stopwatch.StartNew();

        // Arrange
        var step1Started = false;
        var actionCalledCount = 0;

        // Act
        Debounce.Run(10, async () =>
        {
            step1Started = true;
            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step1 Started");

            await Task.Delay(100);
            actionCalledCount++;

            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step1 Completed");
        });

        // Wait for Step1 to start.
        while (!step1Started)
        {
            await Task.Delay(10);
        }

        Debounce.Run(10, async () =>
        {
            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step2 Started");

            await Task.CompletedTask;
            actionCalledCount++;

            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step2 Completed");
        });

        // Wait for the debounce to complete
        await Debounce.CurrentTask;
        await Task.Delay(200);

        // Assert
        Assert.Equal(2, actionCalledCount);
    }
}

#pragma warning restore CS0618 // Type or member is obsolete

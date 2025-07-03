// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class DebounceTests
{
    private readonly ITestOutputHelper Output;

    public DebounceTests(ITestOutputHelper output)
    {
        Output = output;
    }

    [Fact]
    public async Task Debounce_Default()
    {
        var debounce = new Debounce();

        // Arrange
        var actionCalled = false;
        var watcher = Stopwatch.StartNew();

        Assert.False(debounce.IsCompleted);

        // Act
        await debounce.RunAsync(50, async () =>
        {
            actionCalled = true;
            await Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(debounce.IsCompleted);
        Assert.True(watcher.ElapsedMilliseconds >= 50);
        Assert.True(actionCalled);
    }

    [Fact]
    public async Task Debounce_MultipleCalls()
    {
        var debounce = new Debounce();

        // Arrange
        var actionCalledCount = 0;
        var actionCalled = string.Empty;

        // Act 1
        var t1 = debounce.RunAsync(50, async () =>
        {
            actionCalled = "Step1";
            actionCalledCount++;
            await Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        Assert.False(debounce.IsCompleted);

        // Act 2
        var t2 = debounce.RunAsync(40, async () =>
        {
            actionCalled = "Step2";
            actionCalledCount++;
            await Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        await Task.WhenAll(t1, t2);

        // Assert
        Assert.True(debounce.IsCompleted);
        Assert.Equal("Step2", actionCalled);
        Assert.Equal(1, actionCalledCount);
    }

    /************************************/

    [Fact]
    public async Task Debounce_Disposed()
    {
        // Arrange
        var debounce = new Debounce();
        var actionCalled = false;

        // Act
        debounce.Dispose();

        await debounce.RunAsync(50, async () =>
        {
            actionCalled = true;
            await Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(actionCalled);
    }

    [Fact]
    public async Task Debounce_IsCompleted()
    {
        var debounce = new Debounce();

        // Act
        await debounce.RunAsync(50, async () =>
        {
            await Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(debounce.IsCompleted);
    }

    [Fact]
    public async Task Debounce_Cancel()
    {
        await Task.CompletedTask;

        var debounce = new Debounce();

        // Act
        _ = debounce.RunAsync(50, async () =>
        {
            await Task.CompletedTask;
        }, TestContext.Current.CancellationToken);

        debounce.Cancel();

        // Assert
        Assert.False(debounce.IsCompleted);
    }

    [Fact]
    public async Task Debounce_Exception()
    {
        var debounce = new Debounce();

        await Assert.ThrowsAsync<InvalidProgramException>(async () =>
        {
            // Act
            await debounce.RunAsync(50, async () =>
            {
                await Task.CompletedTask;
                throw new InvalidProgramException("Error");     // Simulate an exception
            }, TestContext.Current.CancellationToken);
        });

        // Assert
        Assert.False(debounce.IsCompleted);
    }

    [Fact]
    public async Task Debounce_DelayMustBePositive()
    {
        var debounce = new Debounce();

        // Act
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await debounce.RunAsync(-10, () => Task.CompletedTask, TestContext.Current.CancellationToken);
        });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Debounce_FirstRunAlreadyStarted(bool checkCancellationRequested)
    {
        var debounce = new Debounce();
        var watcher = Stopwatch.StartNew();

        // Arrange
        var step1Started = false;
        var actionCalledCount = 0;

        // Act
        var t1 = debounce.RunAsync(10, async ct =>
        {
            step1Started = true;
            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step1 Started");

            // Simulate some work
            await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

            // Check if cancellation was requested
            if (checkCancellationRequested && ct.IsCancellationRequested)
            {
                Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step1 Cancelled");
                return;
            }

            actionCalledCount++;

            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step1 Completed");
        }, TestContext.Current.CancellationToken);

        // Wait for Step1 to start.
        while (!step1Started)
        {
            await Task.Delay(5, Xunit.TestContext.Current.CancellationToken);
        }

        var t2 = debounce.RunAsync(10, async () =>
        {
            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step2 Started");

            await Task.CompletedTask;
            actionCalledCount++;

            Output.WriteLine($"{watcher.ElapsedMilliseconds}ms: Step2 Completed");
        }, TestContext.Current.CancellationToken);

        // Wait for both tasks to complete
        await Task.WhenAll(t1, t2);

        // Assert
        if (checkCancellationRequested)
        {
            Assert.Equal(1, actionCalledCount);
        }
        else
        {
            Assert.Equal(2, actionCalledCount);
        }
    }
}

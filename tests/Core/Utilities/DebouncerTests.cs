using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class DebouncerTests
{
    [Fact]
    public async Task DebounceAsync_ShouldCallAction_WhenTimerElapsed()
    {
        // Arrange
        var debouncer = new Debouncer();
        var actionCalled = false;

        // Act
        await debouncer.DebounceAsync(100, async () =>
        {
            actionCalled = true;
            await Task.Delay(50);
        });

        // Assert
        Assert.True(actionCalled);
    }

    //[Fact]
    //public async Task DebounceAsync_ShouldNotCallAction_WhenCalledMultipleTimesWithinTimerInterval()
    //{
    //    // Arrange
    //    var debouncer = new Debouncer();
    //    var actionCalledCount = 0;

    //    // Act
    //    await debouncer.DebounceAsync(100, async () =>
    //    {
    //        actionCalledCount++;
    //        await Task.Delay(50);
    //    });
    //    await debouncer.DebounceAsync(100, async () =>
    //    {
    //        actionCalledCount++;
    //        await Task.Delay(50);
    //    });

    //    // Assert
    //    Assert.Equal(1, actionCalledCount);
    //}

    [Fact]
    public async Task DebounceAsync_ShouldNotCallAction_WhenDisposed()
    {
        // Arrange
        var debouncer = new Debouncer();
        var actionCalled = false;

        // Act
        debouncer.Dispose();
        await debouncer.DebounceAsync(100, async () =>
        {
            actionCalled = true;
            await Task.Delay(50);
        });

        // Assert
        Assert.False(actionCalled);
    }

    [Fact]
    public async Task DebounceAsync_Busy()
    {
        // Arrange
        var debouncer = new Debouncer();
        var busy = false;

        // Act
        debouncer.Dispose();
        await debouncer.DebounceAsync(100, async () =>
        {
            busy = debouncer.Busy;
            Assert.True(busy);
            await Task.Delay(50);
        });

        // Assert
        Assert.False(busy);
    }
}

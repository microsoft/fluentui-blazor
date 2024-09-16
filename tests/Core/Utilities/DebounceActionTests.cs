// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.FluentUI.AspNetCore.Components.Utilities.InternalDebounce;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

#pragma warning disable CS0618 // Type or member is obsolete

public class DebounceActionTests
{
    private readonly ITestOutputHelper Output;

    public DebounceActionTests(ITestOutputHelper output)
    {
        Output = output;
    }

    [Fact]
    public async Task Debounce_Default()
    {
        // Arrange
        var debounce = new DebounceAction();
        var actionCalled = false;
        var watcher = Stopwatch.StartNew();

        // Act
        debounce.Run(50, async () =>
        {
            actionCalled = true;
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await debounce.CurrentTask;

        // Assert
        Assert.True(watcher.ElapsedMilliseconds >= 50);
        Assert.True(actionCalled);
    }

    [Fact]
    public async Task Debounce_MultipleCalls()
    {
        // Arrange
        var debounce = new DebounceAction();
        var actionCalledCount = 0;
        var actionCalled = string.Empty;

        // Act
        debounce.Run(50, async () =>
        {
            actionCalled = "Step1";
            actionCalledCount++;
            await Task.CompletedTask;
        });

        debounce.Run(40, async () =>
        {
            actionCalled = "Step2";
            actionCalledCount++;
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await debounce.CurrentTask;

        // Assert
        Assert.Equal("Step2", actionCalled);
        Assert.Equal(1, actionCalledCount);
    }

    [Fact]
    public async Task Debounce_MultipleCalls_Async()
    {
        // Arrange
        var debounce = new DebounceAction();
        var actionCalledCount = 0;
        var actionCalled = string.Empty;

        // Act: simulate two async calls
        _ = Task.Run(() =>
        {
            debounce.Run(50, async () =>
            {
                actionCalled = "Step1";
                actionCalledCount++;
                await Task.CompletedTask;
            });
        });

        await Task.Delay(5);     // To ensure the second call is made after the first one

        _ = Task.Run(() =>
        {
            debounce.Run(40, async () =>
            {
                actionCalled = "Step2";
                actionCalledCount++;
                await Task.CompletedTask;
            });
        });

        await Task.Delay(100);   // Wait for the debounce to complete

        // Assert
        Assert.Equal("Step2", actionCalled);
        Assert.Equal(1, actionCalledCount);
    }

    [Fact]
    public async Task Debounce_Disposed()
    {
        // Arrange
        var debounce = new DebounceAction();
        var actionCalled = false;

        // Act
        debounce.Dispose();

        debounce.Run(50, async () =>
        {
            actionCalled = true;
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await debounce.CurrentTask;

        // Assert
        Assert.False(actionCalled);
    }

    [Fact]
    public async Task Debounce_Busy()
    {
        // Arrange
        var debounce = new DebounceAction();

        // Act
        debounce.Run(50, async () =>
        {
            await Task.CompletedTask;
        });

        // Wait for the debounce to complete
        await debounce.CurrentTask;

        // Assert
        Assert.False(debounce.Busy);
    }

    [Fact]
    public async Task Debounce_Exception()
    {
        // Arrange
        var debounce = new DebounceAction();

        // Act
        debounce.Run(50, async () =>
        {
            await Task.CompletedTask;
            throw new InvalidProgramException("Error");     // Simulate an exception
        });

        // Wait for the debounce to complete
        await debounce.CurrentTask;

        // Assert
        Assert.False(debounce.Busy);
    }

    [Fact]
    public void Debounce_DelayMustBePositive()
    {
        // Arrange
        var debounce = new DebounceAction();

        // Act
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            debounce.Run(-10, () => Task.CompletedTask);
        });
    }

    [Fact]
    public async Task Debounce_FirstRunAlreadyStarted()
    {
        // Arrange
        var debounce = new DebounceAction();
        var actionCalledCount = 0;

        // Act
        debounce.Run(10, async () =>
        {
            Output.WriteLine("Step1 - Started");

            await Task.Delay(100);
            actionCalledCount++;

            Output.WriteLine("Step1 - Completed");
        });

        await Task.Delay(20); // Wait for Step1 to start.

        debounce.Run(10, async () =>
        {
            Output.WriteLine("Step2 - Started");

            await Task.CompletedTask;
            actionCalledCount++;

            Output.WriteLine("Step2 - Completed");
        });

        // Wait for the debounce to complete
        await debounce.CurrentTask;
        await Task.Delay(200);

        // Assert
        Assert.Equal(2, actionCalledCount);
    }
}

#pragma warning restore CS0618 // Type or member is obsolete

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class FluentComponentAutofocusExtensionsTests : BunitContext
{
    private const string FocusIdentifier = "Microsoft.FluentUI.Blazor.Utilities.DomFunctions.focus";

    [Theory]
    [InlineData(false, false, 0)]
    [InlineData(false, true, 0)]
    [InlineData(true, false, 0)]
    [InlineData(true, true, 1)]
    public async Task ApplyAutofocusAsync_RenderAndAutofocus_FocusesOnlyWhenBothEnabled(
        bool firstRender, bool autofocus, int expectedCallCount)
    {
        var component = new AutofocusComponent { Autofocus = autofocus };

        await component.ApplyAutofocusAsync(firstRender);

        Assert.Equal(expectedCallCount, component.FocusCallCount);
        Assert.False(component.PreventScroll);
    }

    [Fact]
    public async Task ApplyAutofocusAsync_NullComponent_ThrowsArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            FluentComponentAutofocusExtensions.ApplyAutofocusAsync(null!, true));

        Assert.Equal("component", exception.ParamName);
    }

    [Fact]
    public async Task ApplyAutofocusAsync_PendingFocus_AwaitsCompletion()
    {
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var component = new AutofocusComponent { Autofocus = true, FocusTask = completion.Task };

        var task = component.ApplyAutofocusAsync(true);

        Assert.Equal(1, component.FocusCallCount);
        Assert.False(task.IsCompleted);
        completion.SetResult();
        await task;
        Assert.True(task.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task ApplyAutofocusAsync_FocusFails_PropagatesException()
    {
        var expected = new JSException("Focus failed.");
        var component = new AutofocusComponent
        {
            Autofocus = true,
            FocusTask = Task.FromException(expected),
        };

        var actual = await Assert.ThrowsAsync<JSException>(() => component.ApplyAutofocusAsync(true));

        Assert.Same(expected, actual);
        Assert.Equal(1, component.FocusCallCount);
    }

    [Fact]
    public async Task FocusElementAsync_NullAutofocusComponent_ThrowsArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            FluentComponentAutofocusExtensions.FocusElementAsync(null!, JSInterop.JSRuntime, "focus-id", false));

        Assert.Equal("component", exception.ParamName);
        Assert.Empty(JSInterop.Invocations);
    }

    [Fact]
    public async Task FocusElementAsync_NullBaseComponent_ThrowsArgumentNullException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            FluentComponentAutofocusExtensions.FocusElementAsync(null!, false));

        Assert.Equal("component", exception.ParamName);
        Assert.Empty(JSInterop.Invocations);
    }

    [Fact]
    public async Task FocusElementAsync_NullRuntime_ThrowsArgumentNullException()
    {
        var component = new AutofocusComponent();

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            component.FocusElementAsync(null!, "focus-id", false));

        Assert.Equal("jsRuntime", exception.ParamName);
        Assert.Empty(JSInterop.Invocations);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    public async Task FocusElementAsync_InvalidId_ThrowsArgumentException(string? id)
    {
        var component = new AutofocusComponent();

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            component.FocusElementAsync(JSInterop.JSRuntime, id, false));

        Assert.Equal("id", exception.ParamName);
        Assert.StartsWith("Component ID cannot be null or whitespace.", exception.Message);
        Assert.Empty(JSInterop.Invocations);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(null, true)]
    [InlineData("", false)]
    [InlineData("", true)]
    [InlineData(" \t\r\n", false)]
    [InlineData(" \t\r\n", true)]
    public async Task FocusElementAsync_BlankSelector_ForwardsOnlyIdAndPreventScroll(string? selector, bool preventScroll)
    {
        var component = new AutofocusComponent();
        JSInterop.SetupVoid(FocusIdentifier, "focus-id", preventScroll).SetVoidResult();

        await component.FocusElementAsync(JSInterop.JSRuntime, "focus-id", preventScroll, selector);

        var invocation = Assert.Single(JSInterop.Invocations);
        Assert.Equal(FocusIdentifier, invocation.Identifier);
        Assert.Equal(new object?[] { "focus-id", preventScroll }, invocation.Arguments);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FocusElementAsync_NonBlankSelector_ForwardsSelector(bool preventScroll)
    {
        var component = new AutofocusComponent();
        const string selector = "#focus-id input";
        JSInterop.SetupVoid(FocusIdentifier, "focus-id", preventScroll, selector).SetVoidResult();

        await component.FocusElementAsync(JSInterop.JSRuntime, "focus-id", preventScroll, selector);

        var invocation = Assert.Single(JSInterop.Invocations);
        Assert.Equal(FocusIdentifier, invocation.Identifier);
        Assert.Equal(new object?[] { "focus-id", preventScroll, selector }, invocation.Arguments);
    }

    [Fact]
    public async Task FocusElementAsync_JavaScriptFails_PropagatesException()
    {
        var component = new AutofocusComponent();
        var expected = new JSException("Focus failed.");
        JSInterop.SetupVoid(FocusIdentifier, "focus-id", false).SetException(expected);

        var actual = await Assert.ThrowsAsync<JSException>(() =>
            component.FocusElementAsync(JSInterop.JSRuntime, "focus-id", false));

        Assert.Same(expected, actual);
        Assert.Single(JSInterop.Invocations);
    }

    private sealed class AutofocusComponent : IFluentComponentAutofocus
    {
        public bool Autofocus { get; set; }

        public Task FocusTask { get; init; } = Task.CompletedTask;

        public int FocusCallCount { get; private set; }

        public bool PreventScroll { get; private set; }

        public Task FocusAsync(bool preventScroll = false)
        {
            FocusCallCount++;
            PreventScroll = preventScroll;
            return FocusTask;
        }
    }
}

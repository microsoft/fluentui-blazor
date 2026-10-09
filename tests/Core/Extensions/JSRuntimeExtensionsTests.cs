// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class JSRuntimeExtensionsTests
{
    private sealed class TestJSRuntime : IJSRuntime
    {
        public Exception? ExceptionToThrow { get; set; }

        public object? Result { get; set; }

        public string? Identifier { get; private set; }

        public object?[]? Arguments { get; private set; }

        public int CallCount { get; private set; }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
            => InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            CallCount++;
            Identifier = identifier;
            Arguments = args;

            if (ExceptionToThrow is not null)
            {
                return ValueTask.FromException<TValue>(ExceptionToThrow);
            }

            return ValueTask.FromResult(Result is null ? default(TValue)! : (TValue)Result);
        }
    }

    public static TheoryData<Exception> HandledExceptions => new()
    {
        new JSDisconnectedException("disconnected"),
        new OperationCanceledException("canceled"),
        new InvalidOperationException("unavailable")
    };

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task InvokeFluentVoidAsync_WhenHandledExceptionOccurs_IgnoresTheException(Exception exception)
    {
        // Arrange
        var jsRuntime = new TestJSRuntime
        {
            ExceptionToThrow = exception
        };

        // Act
        await JSRuntimeExtensions.InvokeFluentVoidAsync(jsRuntime, "test.id", 1, "two");

        // Assert
        Assert.Equal("test.id", jsRuntime.Identifier);
        Assert.Equal(2, jsRuntime.Arguments?.Length);
        Assert.Equal(1, jsRuntime.Arguments![0]);
        Assert.Equal("two", jsRuntime.Arguments[1]);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task InvokeFluentAsync_WhenHandledExceptionOccurs_ReturnsDefaultValue(Exception exception)
    {
        // Arrange
        var jsRuntime = new TestJSRuntime
        {
            ExceptionToThrow = exception
        };

        // Act
        var result = await JSRuntimeExtensions.InvokeFluentAsync<string>(jsRuntime, "get.value", 5);

        // Assert
        Assert.Null(result);
        Assert.Equal("get.value", jsRuntime.Identifier);
        Assert.Equal(1, jsRuntime.Arguments?.Length);
        Assert.Equal(5, jsRuntime.Arguments![0]);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Fact]
    public async Task TryInvokeFluentVoidAsync_WhenInvocationSucceeds_ReturnsTrue()
    {
        // Arrange
        var jsRuntime = new TestJSRuntime();

        // Act
        var success = await jsRuntime.TryInvokeFluentVoidAsync("test.id", 1, "two");

        // Assert
        Assert.True(success);
        Assert.Equal("test.id", jsRuntime.Identifier);
        Assert.Equal(new object?[] { 1, "two" }, jsRuntime.Arguments);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task TryInvokeFluentVoidAsync_WhenHandledExceptionOccurs_ReturnsFalse(Exception exception)
    {
        // Arrange
        var jsRuntime = new TestJSRuntime { ExceptionToThrow = exception };

        // Act
        var success = await jsRuntime.TryInvokeFluentVoidAsync("test.id");

        // Assert
        Assert.False(success);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Theory]
    [InlineData("value")]
    [InlineData("")]
    [InlineData(null)]
    public async Task TryInvokeFluentAsync_WhenInvocationSucceeds_ReturnsSuccessAndValue(string? value)
    {
        // Arrange
        var jsRuntime = new TestJSRuntime { Result = value };

        // Act
        var result = await jsRuntime.TryInvokeFluentAsync<string>("get.value", 5);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(value, result.Value);
        Assert.Equal("get.value", jsRuntime.Identifier);
        Assert.Equal(new object?[] { 5 }, jsRuntime.Arguments);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    public async Task TryInvokeFluentAsync_WhenValueTypeIsReturned_PreservesValue(int value)
    {
        // Arrange
        var jsRuntime = new TestJSRuntime { Result = value };

        // Act
        var result = await jsRuntime.TryInvokeFluentAsync<int>("get.value");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task TryInvokeFluentAsync_WhenHandledExceptionOccurs_ReturnsFailureAndDefaultValue(Exception exception)
    {
        // Arrange
        var jsRuntime = new TestJSRuntime { ExceptionToThrow = exception };

        // Act
        var result = await jsRuntime.TryInvokeFluentAsync<string>("get.value");

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.Value);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Fact]
    public async Task InvokeFluentVoidAsync_WhenInvocationSucceeds_ForwardsInvocation()
    {
        // Arrange
        var jsRuntime = new TestJSRuntime();

        // Act
        await jsRuntime.InvokeFluentVoidAsync("test.id", 1, "two");

        // Assert
        Assert.Equal("test.id", jsRuntime.Identifier);
        Assert.Equal(new object?[] { 1, "two" }, jsRuntime.Arguments);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Fact]
    public async Task InvokeFluentAsync_WhenInvocationSucceeds_ReturnsValue()
    {
        // Arrange
        var jsRuntime = new TestJSRuntime { Result = "value" };

        // Act
        var value = await jsRuntime.InvokeFluentAsync<string>("get.value", 5);

        // Assert
        Assert.Equal("value", value);
        Assert.Equal("get.value", jsRuntime.Identifier);
        Assert.Equal(new object?[] { 5 }, jsRuntime.Arguments);
        Assert.Equal(1, jsRuntime.CallCount);
    }

    [Fact]
    public async Task TryInvokeFluentVoidAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        // Arrange
        var exception = new JSException("script failed");
        var jsRuntime = new TestJSRuntime { ExceptionToThrow = exception };

        // Act
        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsRuntime.TryInvokeFluentVoidAsync("test.id"));

        // Assert
        Assert.Same(exception, actual);
    }

    [Fact]
    public async Task TryInvokeFluentAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        // Arrange
        var exception = new JSException("script failed");
        var jsRuntime = new TestJSRuntime { ExceptionToThrow = exception };

        // Act
        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsRuntime.TryInvokeFluentAsync<string>("get.value"));

        // Assert
        Assert.Same(exception, actual);
    }

    [Fact]
    public async Task InvokeFluentVoidAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        // Arrange
        var exception = new JSException("script failed");
        var jsRuntime = new TestJSRuntime { ExceptionToThrow = exception };

        // Act
        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsRuntime.InvokeFluentVoidAsync("test.id"));

        // Assert
        Assert.Same(exception, actual);
    }

    [Fact]
    public async Task InvokeFluentAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        // Arrange
        var exception = new JSException("script failed");
        var jsRuntime = new TestJSRuntime { ExceptionToThrow = exception };

        // Act
        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsRuntime.InvokeFluentAsync<string>("get.value"));

        // Assert
        Assert.Same(exception, actual);
    }
}

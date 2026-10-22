// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public class JSObjectReferenceExtensionsTests
{
    private sealed class TestJSObjectReference : IJSObjectReference
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

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    public static TheoryData<Exception> HandledExceptions => new()
    {
        new JSDisconnectedException("disconnected"),
        new OperationCanceledException("canceled"),
        new InvalidOperationException("unavailable")
    };

    [Fact]
    public async Task InvokeFluentVoidAsync_WhenInvocationSucceeds_ForwardsInvocation()
    {
        var jsObjectReference = new TestJSObjectReference();

        await jsObjectReference.InvokeFluentVoidAsync("test.id", 1, "two");

        Assert.Equal("test.id", jsObjectReference.Identifier);
        Assert.Equal(new object?[] { 1, "two" }, jsObjectReference.Arguments);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Fact]
    public async Task TryInvokeFluentVoidAsync_WhenInvocationSucceeds_ReturnsTrue()
    {
        var jsObjectReference = new TestJSObjectReference();

        var success = await jsObjectReference.TryInvokeFluentVoidAsync("test.id", 1, "two");

        Assert.True(success);
        Assert.Equal("test.id", jsObjectReference.Identifier);
        Assert.Equal(new object?[] { 1, "two" }, jsObjectReference.Arguments);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Theory]
    [InlineData("value")]
    [InlineData("")]
    [InlineData(null)]
    public async Task InvokeFluentAsync_WhenInvocationSucceeds_ReturnsValue(string? value)
    {
        var jsObjectReference = new TestJSObjectReference { Result = value };

        var result = await jsObjectReference.InvokeFluentAsync<string>("get.value", 5);

        Assert.Equal(value, result);
        Assert.Equal("get.value", jsObjectReference.Identifier);
        Assert.Equal(new object?[] { 5 }, jsObjectReference.Arguments);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Theory]
    [InlineData("value")]
    [InlineData("")]
    [InlineData(null)]
    public async Task TryInvokeFluentAsync_WhenInvocationSucceeds_ReturnsSuccessAndValue(string? value)
    {
        var jsObjectReference = new TestJSObjectReference { Result = value };

        var result = await jsObjectReference.TryInvokeFluentAsync<string>("get.value", 5);

        Assert.True(result.Success);
        Assert.Equal(value, result.Value);
        Assert.Equal("get.value", jsObjectReference.Identifier);
        Assert.Equal(new object?[] { 5 }, jsObjectReference.Arguments);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    public async Task InvokeFluentAsync_WhenValueTypeIsReturned_PreservesValue(int value)
    {
        var jsObjectReference = new TestJSObjectReference { Result = value };

        var result = await jsObjectReference.InvokeFluentAsync<int>("get.value");

        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    public async Task TryInvokeFluentAsync_WhenValueTypeIsReturned_PreservesValue(int value)
    {
        var jsObjectReference = new TestJSObjectReference { Result = value };

        var result = await jsObjectReference.TryInvokeFluentAsync<int>("get.value");

        Assert.True(result.Success);
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task InvokeFluentVoidAsync_WhenHandledExceptionOccurs_IgnoresTheException(Exception exception)
    {
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        await jsObjectReference.InvokeFluentVoidAsync("test.id");

        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task TryInvokeFluentVoidAsync_WhenHandledExceptionOccurs_ReturnsFalse(Exception exception)
    {
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var success = await jsObjectReference.TryInvokeFluentVoidAsync("test.id");

        Assert.False(success);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task InvokeFluentAsync_WhenHandledExceptionOccurs_ReturnsDefaultValue(Exception exception)
    {
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var result = await jsObjectReference.InvokeFluentAsync<string>("get.value");

        Assert.Null(result);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Theory]
    [MemberData(nameof(HandledExceptions))]
    public async Task TryInvokeFluentAsync_WhenHandledExceptionOccurs_ReturnsFailureAndDefaultValue(Exception exception)
    {
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var result = await jsObjectReference.TryInvokeFluentAsync<string>("get.value");

        Assert.False(result.Success);
        Assert.Null(result.Value);
        Assert.Equal(1, jsObjectReference.CallCount);
    }

    [Fact]
    public async Task InvokeFluentVoidAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        var exception = new JSException("script failed");
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsObjectReference.InvokeFluentVoidAsync("test.id"));

        Assert.Same(exception, actual);
    }

    [Fact]
    public async Task TryInvokeFluentVoidAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        var exception = new JSException("script failed");
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsObjectReference.TryInvokeFluentVoidAsync("test.id"));

        Assert.Same(exception, actual);
    }

    [Fact]
    public async Task InvokeFluentAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        var exception = new JSException("script failed");
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsObjectReference.InvokeFluentAsync<string>("get.value"));

        Assert.Same(exception, actual);
    }

    [Fact]
    public async Task TryInvokeFluentAsync_WhenJavaScriptErrorOccurs_PropagatesException()
    {
        var exception = new JSException("script failed");
        var jsObjectReference = new TestJSObjectReference { ExceptionToThrow = exception };

        var actual = await Assert.ThrowsAsync<JSException>(async () =>
            await jsObjectReference.TryInvokeFluentAsync<string>("get.value"));

        Assert.Same(exception, actual);
    }
}

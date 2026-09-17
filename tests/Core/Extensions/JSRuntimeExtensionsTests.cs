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
                throw ExceptionToThrow;
            }

            return ValueTask.FromResult(default(TValue)!);
        }
    }

    [Fact]
    public async Task InvokeFluentVoidAsync_WhenJsRuntimeIsDisconnected_IgnoresTheException()
    {
        // Arrange
        var jsRuntime = new TestJSRuntime
        {
            ExceptionToThrow = new JSDisconnectedException("disconnected")
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

    [Fact]
    public async Task InvokeFluentAsync_WhenJsRuntimeIsDisconnected_ReturnsDefaultValue()
    {
        // Arrange
        var jsRuntime = new TestJSRuntime
        {
            ExceptionToThrow = new JSDisconnectedException("disconnected")
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
}

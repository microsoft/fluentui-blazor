namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Utilities;

public class JSModuleTests
{
    //[Fact]
    //public async Task InvokeVoidAsync_InvokesMethod()
    //{
    //    // Arrange
    //    var jsRuntime = new MockJSRuntime();
    //    var module = new TestJSModule(jsRuntime, "test.js");

    //    // Act
    //    await module.InvokeVoidAsync("testMethod");

    //    // Assert
    //    Assert.True(jsRuntime.InvokedMethods.Contains(("import", new object[] { "test.js" })));
    //    Assert.True(jsRuntime.InvokedMethods.Contains(("testMethod", null)));
    //}

    //[Fact]
    //public async Task InvokeAsync_InvokesMethod()
    //{
    //    // Arrange
    //    var jsRuntime = new MockJSRuntime();
    //    var module = new TestJSModule(jsRuntime, "test.js");

    //    // Act
    //    var result = await module.InvokeAsync<string>("testMethod");

    //    // Assert
    //    Assert.True(jsRuntime.InvokedMethods.Contains(("import", new object[] { "test.js" })));
    //    Assert.True(jsRuntime.InvokedMethods.Contains(("testMethod", null)));
    //    Assert.Equal("testResult", result);
    //}

    //[Fact]
    //public async Task DisposeAsync_DisposesModule()
    //{
    //    // Arrange
    //    var jsRuntime = new MockJSRuntime();
    //    var module = new TestJSModule(jsRuntime, "test.js");

    //    // Act
    //    await module.DisposeAsync();

    //    // Assert
    //    Assert.True(jsRuntime.InvokedMethods.Contains(("import", new object[] { "test.js" })));
    //    Assert.True(jsRuntime.InvokedMethods.Contains(("dispose", null)));
    //}

    //private class TestJSModule : JSModule
    //{
    //    public TestJSModule(IJSRuntime js, string moduleUrl) : base(js, moduleUrl)
    //    {
    //    }

    //    public async Task InvokeVoidAsync(string method)
    //    {
    //        await Task.Delay(100);

    //    }

    //    public new ValueTask DisposeAsync()
    //    {
    //        return base.DisposeAsync();
    //    }
    //}

    //public class MockJSRuntime : IJSRuntime
    //{
    //    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
    //    {
    //        return default;
    //    }

    //    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    //    {
    //        return default;
    //    }
    //}
}

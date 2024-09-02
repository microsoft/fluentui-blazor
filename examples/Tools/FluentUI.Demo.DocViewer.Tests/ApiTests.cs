// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Tests.Extensions;
using FluentUI.Demo.DocViewer.Tests.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Xunit;

namespace FluentUI.Demo.DocViewer.Tests;

public class ApiTests
{
    private readonly DocViewerServiceTests DocViewerService = new();

    [Fact]
    public void Api_Default()
    {
        var api = new ApiClass(DocViewerService, typeof(MyComponent));

        Assert.Single(api.Properties);
        Assert.Equal("Age", api.Properties.First().Name);

        Assert.Single(api.Events);
        Assert.Equal("OnAgeChanged", api.Events.First().Name);

        Assert.Single(api.Methods);
        Assert.Equal("GetAge", api.Methods.First().Name);
        Assert.Equal("string GetAge(int increment)", api.Methods.First().GetMethodSignature());
    }

    [Fact]
    public void Api_Generic()
    {
        var api = new ApiClass(DocViewerService, typeof(MyGenericComponent<>));
        api.InstanceTypes = [typeof(int)];

        Assert.Single(api.Properties);
        Assert.Single(api.Events);
        Assert.Single(api.Methods);

        Assert.Equal("GetValue<U>", api.Methods.First().Name);
    }

    public class MyComponent : ComponentBase
    {
        [Parameter]
        public int Age { get; set; } = 0;

        [Parameter]
        [Obsolete]
        public int OldAge { get; set; } = 0;

        [Parameter]
        public EventCallback<int> OnAgeChanged { get; set; }

        public string GetAge(int increment) => (Age + increment).ToString(CultureInfo.InvariantCulture);

        [JSInvokable]
        public string CalledFromJs(int increment) => GetAge(increment);
    }

    public class MyGenericComponent<T> : ComponentBase where T : struct
    {
        [Parameter]
        public T? Value { get; set; }

        public T? GetValue<U>() => Value;

        [Parameter]
        public EventCallback<T> OnChanged { get; set; }
    }
}

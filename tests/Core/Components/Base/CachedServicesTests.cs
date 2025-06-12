// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class CachedServicesTests : Bunit.TestContext
{
    public CachedServicesTests()
    {
        Services.AddScoped<LibraryConfiguration>();
    }

    [Fact]
    public void CachedServices_GetCachedServiceOrNull()
    {
        var tooltip = new TooltipService();
        Services.AddScoped<ITooltipService>(factory => tooltip);

        // Arrange
        var cachedServices = new CachedServices(Services);

        // Act - First call => To Cache
        var service1 = cachedServices.GetCachedServiceOrNull<ITooltipService>();
        Assert.Equal(tooltip, service1);

        // Act - Second call => From cache
        var service2 = cachedServices.GetCachedServiceOrNull<ITooltipService>();
        Assert.Equal(tooltip, service2);

        // Act - Unknown service => null
        var service3 = cachedServices.GetCachedServiceOrNull<IFluentLocalizer>();
        Assert.Null(service3);
    }
     
    [Fact]
    public async Task CachedServices_RenderTooltipAsync_LabelNull()
    {
        Services.AddScoped<ITooltipService, TooltipService>();

        // Arrange
        var cachedServices = new CachedServices(Services);
        var button = new FluentButton(default!);

        // Act
        await cachedServices.RenderTooltipAsync(button, label: null);

        // Assert
        Assert.Null(button.Id);
    }

    [Fact]
    public async Task CachedServices_RenderTooltipAsync_LabelNotNull()
    {
        Services.AddScoped<ITooltipService, TooltipService>();

        // Arrange
        var cachedServices = new CachedServices(Services);
        var button = new FluentButton(default!);

        // Act
        await cachedServices.RenderTooltipAsync(button, label: "abc");

        // Assert
        Assert.NotNull(button.Id);
    }

    [Fact]
    public async Task CachedServices_RenderTooltipAsync_NoService()
    {
        // Arrange
        var cachedServices = new CachedServices(Services);
        var button = new FluentButton(default!);

        // Act
        await cachedServices.RenderTooltipAsync(button, label: "abc");

        // Assert
        Assert.Null(button.Id);
    }
}

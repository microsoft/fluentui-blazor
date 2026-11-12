// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class CachedServicesTests : Bunit.BunitContext
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

    [Fact]
    public async Task CachedServices_DisposeTooltipAsync_RemovesTooltip()
    {
        ITooltipService tooltipService = new TooltipService();
        Services.AddScoped(factory => tooltipService);

        // Arrange
        var cachedServices = new CachedServices(Services);
        var button = new FluentButton(default!);
        await cachedServices.RenderTooltipAsync(button, label: "abc");

        // Act
        await cachedServices.DisposeTooltipAsync(button);

        // Assert
        Assert.Empty(tooltipService.Items);
    }

    [Fact]
    public async Task CachedServices_DisposeTooltipAsync_ServiceProviderDisposed()
    {
        using var provider = new ServiceCollection()
            .AddScoped<LibraryConfiguration>()
            .AddScoped<ITooltipService, TooltipService>()
            .BuildServiceProvider();
        using var scope = provider.CreateScope();
        var tooltipService = scope.ServiceProvider.GetRequiredService<ITooltipService>();

        // Arrange
        var cachedServices = new CachedServices(scope.ServiceProvider);
        var button = new FluentButton(default!);
        await cachedServices.RenderTooltipAsync(button, label: "abc");
        scope.Dispose();

        // Act
        var exception = await Record.ExceptionAsync(() => cachedServices.DisposeTooltipAsync(button));

        // Assert
        Assert.Null(exception);
        Assert.Empty(tooltipService.Items);
    }

    [Fact]
    public async Task CachedServices_DisposeTooltipAsync_ServiceNeverResolved()
    {
        using var provider = new ServiceCollection()
            .AddScoped<LibraryConfiguration>()
            .AddScoped<ITooltipService, TooltipService>()
            .BuildServiceProvider();
        using var scope = provider.CreateScope();

        // Arrange
        var cachedServices = new CachedServices(scope.ServiceProvider);
        IFluentComponentBase button = new FluentButton(default!);
        button.Id = "my-button";
        scope.Dispose();

        // Act
        var exception = await Record.ExceptionAsync(() => cachedServices.DisposeTooltipAsync(button));

        // Assert
        Assert.Null(exception);
    }
}

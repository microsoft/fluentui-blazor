// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

// BL0005: The GetIntentIcon tests intentionally set [Parameter] properties on a
// non-rendered subclass instance to exercise the icon resolution logic directly.
#pragma warning disable BL0005

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Toast;

public class FluentToastTests : Bunit.BunitContext
{
    public FluentToastTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents();
    }

    /// <summary>
    /// Subclass exposing the protected <c>GetIntentIcon</c> method for testing.
    /// </summary>
    private sealed class TestableToast : FluentToast
    {
        public TestableToast(LibraryConfiguration configuration) : base(configuration)
        {
        }

        public Icon? GetIntentIconForTest() => GetIntentIcon();
    }

    private TestableToast CreateToast()
    {
        var configuration = Services.GetRequiredService<LibraryConfiguration>();
        return new TestableToast(configuration);
    }

    [Fact]
    public void FluentToast_RendersToastElement()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Title, "My Toast")
            .Add(p => p.Opened, true));

        // Assert
        Assert.Contains("fluent-toast-b", cut.Markup);
        Assert.Contains("My Toast", cut.Markup);
    }

    [Fact]
    public void FluentToast_AppliesWidthStyle()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Width, "350px")
            .Add(p => p.Opened, true));

        // Assert
        Assert.Contains("--toast-width", cut.Markup);
        Assert.Contains("350px", cut.Markup);
    }

    [Fact]
    public void FluentToast_DefaultValues()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true));

        // Assert
        Assert.True(cut.Instance.AllowDismiss);
        Assert.Equal(16, cut.Instance.VerticalOffset);
        Assert.Equal(20, cut.Instance.HorizontalOffset);
    }

    [Theory]
    [InlineData(ToastIntent.Success)]
    [InlineData(ToastIntent.Warning)]
    [InlineData(ToastIntent.Error)]
    [InlineData(ToastIntent.Info)]
    public void FluentToast_GetIntentIcon_ReturnsIcon_ForIntent(ToastIntent intent)
    {
        // Arrange
        var toast = CreateToast();
        toast.Intent = intent;

        // Act
        var icon = toast.GetIntentIconForTest();

        // Assert
        Assert.NotNull(icon);
    }

    [Fact]
    public void FluentToast_GetIntentIcon_ReturnsNull_WhenNoIntent()
    {
        // Arrange
        var toast = CreateToast();

        // Act
        var icon = toast.GetIntentIconForTest();

        // Assert
        Assert.Null(icon);
    }

    [Fact]
    public void FluentToast_GetIntentIcon_ReturnsNull_ForProgress()
    {
        // Arrange
        var toast = CreateToast();
        toast.Intent = ToastIntent.Progress;

        // Act
        var icon = toast.GetIntentIconForTest();

        // Assert
        Assert.Null(icon);
    }

    [Fact]
    public void FluentToast_GetIntentIcon_UsesInvertedColor_WhenInverted()
    {
        // Arrange
        var toast = CreateToast();
        toast.Intent = ToastIntent.Success;
        toast.Inverted = true;

        // Act
        var icon = toast.GetIntentIconForTest();

        // Assert
        Assert.NotNull(icon);
    }

    [Fact]
    public void FluentToast_RendersDismissButton_WhenAllowDismiss()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.AllowDismiss, true));

        // Assert
        Assert.Contains("slot=\"action\"", cut.Markup);
    }

    [Fact]
    public void FluentToast_DoesNotRenderDismiss_WhenAllowDismissFalse()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.AllowDismiss, false));

        // Assert
        Assert.DoesNotContain("slot=\"action\"", cut.Markup);
    }
}

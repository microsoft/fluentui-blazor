// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components;
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

    [Fact]
    public void FluentToast_RendersSpinner_ForProgressIntent()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.Intent, ToastIntent.Progress));

        // Assert
        Assert.Contains("slot=\"media\"", cut.Markup);
        Assert.Contains("fluent-spinner", cut.Markup);
    }

    [Fact]
    public void FluentToast_RendersIntentIcon_ForInfoIntent()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.Intent, ToastIntent.Info));

        // Assert
        Assert.Contains("slot=\"media\"", cut.Markup);
    }

    [Fact]
    public void FluentToast_RendersCustomIcon_WhenIconSet()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.Icon, new CoreIcons.Regular.Size20.Info()));

        // Assert
        Assert.Contains("slot=\"media\"", cut.Markup);
    }

    [Fact]
    public void FluentToast_RendersDismissLink_WhenDismissLabelSet()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.AllowDismiss, true)
            .Add(p => p.DismissAction, new ToastOptionsAction { Label = "Close" }));

        // Assert
        Assert.Contains("slot=\"action\"", cut.Markup);
        Assert.Contains("Close", cut.Markup);
    }

    [Fact]
    public void FluentToast_RendersSubtitle_WhenSubtitleSet()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.Subtitle, "My subtitle"));

        // Assert
        Assert.Contains("slot=\"subtitle\"", cut.Markup);
        Assert.Contains("My subtitle", cut.Markup);
    }

    [Fact]
    public void FluentToast_RendersFooterTemplate_WhenSet()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.FooterTemplate, (RenderFragment)(builder => builder.AddContent(0, "My footer"))));

        // Assert
        Assert.Contains("slot=\"footer\"", cut.Markup);
        Assert.Contains("My footer", cut.Markup);
    }

    [Fact]
    public void FluentToast_RendersChildContent_WhenSet()
    {
        // Arrange & Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.ChildContent, (RenderFragment)(builder => builder.AddContent(0, "My body"))));

        // Assert
        Assert.Contains("My body", cut.Markup);
        Assert.Contains("-body", cut.Markup);
    }

    [Fact]
    public void FluentToast_DismissButton_ClosesToast_WhenNoInstance()
    {
        // Arrange
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.AllowDismiss, true));

        // Act
        cut.Find("fluent-button").Click();

        // Assert
        Assert.False(cut.Instance.Opened);
    }

    [Fact]
    public void FluentToast_DismissButton_IsNoOp_WhenAlreadyClosed()
    {
        // Arrange
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, false)
            .Add(p => p.AllowDismiss, true));

        // Act
        cut.Find("fluent-button").Click();

        // Assert
        Assert.False(cut.Instance.Opened);
    }

    [Fact]
    public void FluentToast_WithInstance_OpensOnFirstRender_AndTogglesVisible()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions { Id = "toast-open", Title = "Hello" });

        // Act
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, false)
            .AddCascadingValue<IToastInstance>(instance));

        // Assert: OnAfterRenderAsync opens the toast when an instance is cascaded.
        Assert.True(cut.Instance.Opened);

        // Act: notify the component that the underlying dialog is opened.
        cut.Find("fluent-toast-b").TriggerEvent("ondialogtoggle", new DialogToggleEventArgs
        {
            Id = "toast-open",
            Type = "toggle",
            NewState = "open",
        });

        // Assert
        Assert.Equal(ToastLifecycleStatus.Visible, instance.LifecycleStatus);
    }

    [Fact]
    public async Task FluentToast_WithInstance_ToggleClosed_DismissesAndCompletesResult()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions { Id = "toast-close", Title = "Bye" });

        bool? openedChangedValue = null;
        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.OpenedChanged, EventCallback.Factory.Create<bool>(this, value => openedChangedValue = value))
            .AddCascadingValue<IToastInstance>(instance));

        // Act
        cut.Find("fluent-toast-b").TriggerEvent("ondialogtoggle", new DialogToggleEventArgs
        {
            Id = "toast-close",
            Type = "toggle",
            NewState = "closed",
        });

        // Assert
        Assert.False(cut.Instance.Opened);
        Assert.False(openedChangedValue);
        Assert.Equal(ToastLifecycleStatus.Dismissed, instance.LifecycleStatus);

        var result = await instance.Result;
        Assert.Equal(ToastCloseReason.TimedOut, result.Reason);
    }

    [Fact]
    public void FluentToast_Toggle_IsIgnored_WhenIdDoesNotMatch()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions { Id = "toast-id", Title = "Title" });

        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .AddCascadingValue<IToastInstance>(instance));

        // Act: send a toggle event for a different toast id.
        cut.Find("fluent-toast-b").TriggerEvent("ondialogtoggle", new DialogToggleEventArgs
        {
            Id = "other-id",
            Type = "toggle",
            NewState = "closed",
        });

        // Assert: the mismatched event is ignored and nothing changes.
        Assert.True(cut.Instance.Opened);
        Assert.Equal(ToastLifecycleStatus.Unmounted, instance.LifecycleStatus);
    }

    [Fact]
    public void FluentToast_WithInstance_DismissButton_InvokesDismissAction()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions { Id = "toast-action", Title = "Title" });

        var invoked = false;
        var dismissAction = new ToastOptionsAction
        {
            OnClickAsync = _ =>
            {
                invoked = true;
                return Task.CompletedTask;
            },
        };

        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.AllowDismiss, true)
            .Add(p => p.DismissAction, dismissAction)
            .AddCascadingValue<IToastInstance>(instance));

        // Act
        cut.Find("fluent-button").Click();

        // Assert
        Assert.True(invoked);
    }

    [Fact]
    public void FluentToast_WithInstance_DismissButton_ClosesInstance()
    {
        // Arrange
        var service = (NotificationService)Services.GetRequiredService<INotificationService>();
        var instance = new ToastInstance(service, new ToastOptions { Id = "toast-dismiss", Title = "Title" });

        var cut = Render<FluentToast>(parameters => parameters
            .Add(p => p.Opened, true)
            .Add(p => p.AllowDismiss, true)
            .AddCascadingValue<IToastInstance>(instance));

        // Make the toast visible so the service does not ignore the close request.
        cut.Find("fluent-toast-b").TriggerEvent("ondialogtoggle", new DialogToggleEventArgs
        {
            Id = "toast-dismiss",
            Type = "toggle",
            NewState = "open",
        });

        // Act
        cut.Find("fluent-button").Click();

        // Assert
        Assert.Equal(ToastLifecycleStatus.Dismissed, instance.LifecycleStatus);
    }
}

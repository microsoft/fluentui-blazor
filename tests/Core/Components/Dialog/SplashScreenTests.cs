// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Dialog;

public class SplashScreenTests : BunitContext
{
    public SplashScreenTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddFluentUIComponents(options => options.UseGlobalOverlay = false);
    }

    [Fact]
    public void FluentSplashScreen_RendersContent()
    {
        // Arrange
        var content = new SplashScreenContent
        {
            Title = "Loading app",
            SubTitle = "Preparing your session",
            LoadingText = "Fetching configuration",
            Message = (MarkupString)"Please wait a moment.",
            Logo = "https://example.com/logo.svg",
            DisplayTime = 0
        };

        // Act
        var cut = Render<FluentSplashScreen>(parameters => parameters
            .Add(p => p.Content, content));

        // Assert
        Assert.Contains("Loading app", cut.Markup);
        Assert.Contains("Preparing your session", cut.Markup);
        Assert.Contains("Fetching configuration", cut.Markup);
        Assert.Contains("Please wait a moment.", cut.Markup);
        Assert.Equal("https://example.com/logo.svg", cut.Find("img.icon").GetAttribute("src"));
    }

    [Fact]
    public void FluentSplashScreen_LogoIsSvg_RendersInlineSvg()
    {
        // Arrange
        const string svgLogo = "<svg viewBox=\"0 0 16 16\" role=\"img\"><circle cx=\"8\" cy=\"8\" r=\"6\" /></svg>";
        var content = new SplashScreenContent
        {
            Logo = svgLogo,
            DisplayTime = 0
        };

        // Act
        var cut = Render<FluentSplashScreen>(parameters => parameters
            .Add(p => p.Content, content));

        // Assert
        var icon = cut.Find("div.icon");
        var svg = icon.QuerySelector("svg");
        Assert.NotNull(svg);
        Assert.Equal("0 0 16 16", svg.GetAttribute("viewBox"));
        Assert.Equal("img", svg.GetAttribute("role"));
        Assert.Empty(cut.FindAll("img.icon"));
    }

    [Fact]
    public void SplashScreenContent_UpdateLabels_RefreshesProperties()
    {
        // Arrange
        var content = new SplashScreenContent();
        var refreshCount = 0;
        content.RefreshProperties = () => refreshCount++;

        // Act
        content.UpdateLabels("Loading your data", (MarkupString)"Finalizing...");

        // Assert
        Assert.Equal("Loading your data", content.LoadingText);
        Assert.Equal("Finalizing...", content.Message?.Value);
        Assert.Equal(1, refreshCount);
    }

    [Fact]
    public async Task ShowSplashScreenAsync_ConfiguresDefaultDialogOptions()
    {
        // Arrange
        Render<FluentDialogProvider>();
        var dialogService = (DialogService)Services.GetRequiredService<IDialogService>();
        var content = new SplashScreenContent
        {
            Title = "Loading app",
            DisplayTime = 750
        };

        // Act
        var resultTask = dialogService.ShowSplashScreenAsync(content);
        var instance = Assert.Single(dialogService.ServiceProvider.Items.Values);
        var dialogInstance = Assert.IsType<DialogInstance>(instance);

        // Assert
        Assert.Equal(typeof(FluentSplashScreen), ((IDialogInstance)dialogInstance).ComponentType);
        Assert.Equal("600px", dialogInstance.Options.Width);
        Assert.Equal("370px", dialogInstance.Options.Height);
        Assert.True(dialogInstance.Options.Modal);
        Assert.True(dialogInstance.Options.PreventDismissOnEscape);
        Assert.Same(content, dialogInstance.Options.Parameters[nameof(FluentSplashScreen.Content)]);

        await dialogInstance.CloseAsync();
        var result = await resultTask;
        Assert.False(result.Cancelled);
        Assert.Null(result.Value);
    }
}

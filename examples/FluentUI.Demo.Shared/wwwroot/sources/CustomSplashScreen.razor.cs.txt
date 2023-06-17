using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared;

public partial class CustomSplashScreen : ISplashScreenParameters, IDialogContentComponent
{
    /// <summary>
    /// Typically used to show the product name.
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = "Your Product name"; //SplashScreenResources.ProductName;

    /// <summary>
    /// Typically used to show the name of the suite the product belongs to.
    /// </summary>
    [Parameter]
    public string? SubTitle { get; set; } = "Your Suite name"; //SplashScreenResources.SuiteName;

    /// <summary>
    /// Text to indicate something is happening.
    /// </summary>
    [Parameter]
    public string? LoadingText { get; set; } = "Loading..."; //SplashScreenResources.LoadingLabel;

    /// <summary>
    /// An extra message. 
    /// </summary>
    [Parameter]
    public MarkupString? Message { get; set; }

    /// <summary>
    /// Logo to show on the splash screen.
    /// Can be a URL or a base64 encoded string or an SVG.
    /// </summary>
    [Parameter]
    public string? Logo { get; set; }

    /// <summary>
    /// Width of the splash screen. 
    /// Should be a valid CSS width string like "600px" or "3em".
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Height of the splash screen. 
    /// Should be a valid CSS height string like "600px" or "3em".
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Callback for when the dialog is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; }

    [CascadingParameter]
    public FluentDialog ParentDialog { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Simulation of loading process
            await Task.Delay(4000);

            // Close the dialog
            await ParentDialog.CloseAsync();
        }
    }
}

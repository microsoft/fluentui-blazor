// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays a loading splash screen as a modal dialog.
/// </summary>
public partial class FluentSplashScreen : ComponentBase
{

    /// <summary>
    /// Gets or sets the content to display in the splash screen.
    /// </summary>
    [Parameter]
    public SplashScreenContent Content { get; set; } = default!;

    /// <summary>
    /// Gets the dialog instance for this splash screen.
    /// </summary>
    [CascadingParameter]
    public IDialogInstance DialogInstance { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Content is not null)
        {
            Content.RefreshProperties = () => _ = InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || Content is null || Content.DisplayTime <= 0)
        {
            return;
        }

        await Task.Delay(Content.DisplayTime);
        await DialogInstance.CloseAsync();
    }
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides a mechanism for handling errors in a fluent manner within a specific context.
/// </summary>
public partial class FluentErrorBoundary : FluentComponentBase
{
    private ErrorBoundary? ErrorBoundary;

    /// <summary>
    /// Gets or sets the icon displayed in the title bar to indicate an error state.
    /// </summary>
    public static Icon TitleIcon { get; set; } = new CoreIcons.Filled.Size20.DismissCircle().WithColor(Color.Error);

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the system should automatically attempt to recover from errors.
    /// Default is true. You can use the <see cref="Recover"/> method to manually recover from an error.
    /// </summary>
    [Parameter]
    public bool AutoRecover { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether child content should be hidden when an error occurs.
    /// Using False is not recommended, as it may cause performance and security issues:
    /// If the detected error is triggered when the content is displayed, there is a risk of an infinite loop.
    /// </summary>
    [Parameter]
    public bool HideChildContentOnError { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered as the dialog header for error messages.
    ///  This content will be rendered in place of the default dialog header when an error occurs within the boundary.
    /// </summary>
    [Parameter]
    public RenderFragment? ErrorHeader { get; set; }

    /// <summary>
    /// Gets or sets the content to be displayed when there is an error. This content will be rendered in a dialog box.
    /// This content will be rendered in place of the default error message when an error occurs within the boundary.
    /// </summary>
    [Parameter]
    public RenderFragment? ErrorContent { get; set; }

    /// <summary>
    /// Gets or sets the details to display when an error occurs within the error boundary.
    /// </summary>
    [Parameter]
    public ErrorBoundaryDetails DisplayErrorDetails { get; set; } = ErrorBoundaryDetails.None;

    /// <summary>
    /// Gets or sets the maximum number of errors that can be handled. If more errors are received,
    /// they will be treated as fatal. Calling <see cref="Recover"/> resets the count.
    /// </summary>
    [Parameter]
    public int MaximumErrorCount { get; set; } = 100;

    /// <summary>
    /// Resets the error boundary to a non-errored state. If the error boundary is not
    /// already in an errored state, the call has no effect.
    /// </summary>
    public void Recover()
    {
        try
        {
            ErrorBoundary?.Recover();
        }
        catch (Exception)
        {
        }
    }

    /// <summary />
    internal Task OnToggleAsync(DialogToggleEventArgs e)
    {
        if (AutoRecover && string.Equals(e.NewState, "closed", StringComparison.Ordinal))
        {
            Recover();
        }

        return Task.CompletedTask;
    }
}

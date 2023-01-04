using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialog : FluentComponentBase
{
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .AddStyle("position", "absolute")
        .AddStyle("z-index", "5")
        .Build();

    /// <summary>
    /// Indicates the element is modal. When modal, user mouse interaction will be limited to the contents of the element by a modal
    /// overlay.  Clicks on the overlay will cause the dialog to emit a "dismiss" event.
    /// </summary>
    [Parameter]
    public bool Modal { get; set; } = true;

    /// <summary>
    /// Gets or sets if the dialog is hidden
    /// </summary>
    [Parameter]
    public bool Hidden { get; set; }

    /// <summary>
    /// Indicates that the dialog should trap focus.
    /// </summary>
    [Parameter]
    public bool TrapFocus { get; set; } = true;

    /// <summary>
    /// The id of the element describing the dialog.
    /// </summary>
    [Parameter]
    public string? AriaDescribedby { get; set; }

    /// <summary>
    /// The id of the element labeling the dialog.
    /// </summary>
    [Parameter]
    public string? AriaLabelledby { get; set; }

    /// <summary>
    /// The label surfaced to assistive technologies.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogEventArgs))]

    public FluentDialog()
    {
        
    }

    public void Show()
    {
        Hidden = false;
        StateHasChanged();
    }

    public void Hide()
    {
        Hidden = true;
        StateHasChanged();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        Element.FocusAsync();
        base.OnAfterRender(firstRender);
    }
}
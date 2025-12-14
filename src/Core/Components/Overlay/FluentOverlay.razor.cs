// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents an overlay component that provides visual layering and interaction blocking in Fluent UI applications.
/// </summary>
public partial class FluentOverlay : FluentComponentBase, IFluentComponentElementBase
{
    /// <summary />
    public FluentOverlay(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is displayed in full screen mode.
    /// </summary>
    [Parameter]
    public bool FullScreen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay allows interaction with underlying content.
    /// </summary>
    [Parameter]
    public bool Interactive { get; set; }

    /// <summary />
    [Parameter]
    public string CloseMode { get; set; } = "all";

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the visibility state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Displays the overlay.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public async Task ShowAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Show", Id);
    }

    /// <summary>
    /// Hides the overlay.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public async Task CloseAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Dialog.Close", Id);
    }

    /// <summary />
    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        if (string.CompareOrdinal(args.Id, Id) != 0)
        {
            return;
        }

        var isVisible = string.Equals(args.NewState, "open", StringComparison.OrdinalIgnoreCase);
        if (Visible != isVisible)
        {
            Visible = isVisible;

            if (VisibleChanged.HasDelegate)
            {
                await VisibleChanged.InvokeAsync(Visible);
            }
        }
    }
}

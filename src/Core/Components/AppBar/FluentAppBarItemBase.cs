// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;
public abstract class FluentAppBarItemBase : FluentComponentBase, IAppBarItem, IDisposable
{
    private bool? _overflow;

    /// <summary>
    /// Gets or sets the URL for this item.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets how the link should be matched.
    /// Defaults to <see cref="NavLinkMatch.Prefix"/>.
    /// </summary>
    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    /// <summary>
    /// Gets or sets the Icon to use when the item is not hovered/selected/active.
    /// </summary>
    [Parameter, EditorRequired]
    public required Icon IconRest { get; set; }

    /// <summary>
    /// Gets or sets the Icon to use when the item is hovered/selected/active.
    /// </summary>
    [Parameter]
    public Icon? IconActive { get; set; }

    /// <summary>
    /// Gets or sets the text to show under the icon.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tooltip to show when the item is hovered.
    /// </summary>
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the count to show on the item with a <see cref="FluentCounterBadge"/>.
    /// </summary>
    [Parameter]
    public int Count { get; set; } = 0;

    /// <summary>
    /// Gets or sets the position of the item in the app bar.
    /// </summary>
    [Parameter]
    public int Order { get; set; }

    /// <summary>
    ///  Gets or sets the content to be shown.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The callback to invoke when the item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FluentAppBarItem> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentAppBar component.
    /// </summary>
    [CascadingParameter]
    private InternalAppBarContext Owner { get; set; } = default!;

    /// <summary>
    /// If this app is outside of visible app bar area.
    /// </summary>
    public bool? Overflow
    {
        get => _overflow;
        set => _overflow = value;
    }


    public FluentAppBarItemBase()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Owner!.Register(this);

        if (string.IsNullOrWhiteSpace(Href))
        {
            Match = NavLinkMatch.All;
        }
    }

    internal void SetOverflow(bool? overflow)
    {
        Overflow = overflow == true ? overflow : null;
    }

    public void Dispose()
    {
        Owner?.Unregister(this);
    }
}

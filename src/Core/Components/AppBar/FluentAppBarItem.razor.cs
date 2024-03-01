using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentAppBarItem : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the URL for this item.
    /// </summary>
    [Parameter, EditorRequired]
    public required string Href { get; set; }

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
    ///  Gets or sets the content to be shown.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentAppBar component.
    /// </summary>
    [CascadingParameter]
    public FluentAppBar Owner { get; set; } = default!;

    /// <summary>
    /// If this app is outside of visible app bar area.
    /// </summary>
    public bool? Overflow { get; private set; }

    internal string? ClassValue => new CssBuilder("appbar-item")
        .AddClass(Class)
        .Build();

    public FluentAppBarItem()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Owner!.Register(this);
    }

    internal void SetProperties(bool? overflow)
    {
        Overflow = overflow == true ? overflow : null;
    }

    public void Dispose()
    {
        Owner?.Unregister(this);
    }
}

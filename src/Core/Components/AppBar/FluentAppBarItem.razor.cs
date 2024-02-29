

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentAppBarItem : FluentComponentBase, IDisposable
{
    [Parameter, EditorRequired]
    public required string Href { get; set; }

    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    [Parameter, EditorRequired]
    public required Icon Icon { get; set; }

    [Parameter]
    public Icon? SecondaryIcon { get; set; }

    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Tooltip { get; set; }

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

    /// <summary />
    internal void SetProperties(bool? overflow)
    {
        Overflow = overflow == true ? overflow : null;
    }

    public void Dispose()
    {
        Owner?.Unregister(this);
    }
}

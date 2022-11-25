using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTab : FluentComponentBase, IDisposable
{
    internal string TabId { get; } = Identifier.NewId();

    /// <summary>
    /// When true, the control will be immutable by user interaction. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the text of the tab
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTabs component
    /// </summary>
    [CascadingParameter]
    public FluentTabs Owner { get; set; } = default!;


    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    public void Dispose() => Owner?.Unregister(this);
}
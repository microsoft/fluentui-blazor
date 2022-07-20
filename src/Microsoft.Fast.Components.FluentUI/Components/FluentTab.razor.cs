using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTab : FluentComponentBase, IDisposable
{
    internal string TabId { get; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the text of the tab
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTabs component
    /// </summary>
    [CascadingParameter]
    public FluentTabs Owner { get; set; } = default!;

    [Parameter]
    public bool Disabled { get; set; } = false;

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    public void Dispose()
    {
        Owner.Unregister(this);
    }
}
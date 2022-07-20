using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeItem : FluentComponentBase, IDisposable
{
    internal string TreeItemId { get; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the owning FluentTreeView
    /// </summary>
    [CascadingParameter]
    public FluentTreeView Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the text of the tree item
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets if the tree item is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets if the tree item is selected
    /// </summary>
    [Parameter]
    public bool Selected { get; set; } = false;

    /// <summary>
    /// Gets or sets if the tree item is expanded (true) or folded (false)
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; } = false;

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    public void Dispose()
    {
        Owner.Unregister(this);
    }
}
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
    /// When true, the control will be appear expanded by user interaction.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// When true, the control will appear selected by user interaction.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// When true, the control will be immutable by user interaction. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    public void Dispose()
    {
        Owner?.Unregister(this);
    }
}
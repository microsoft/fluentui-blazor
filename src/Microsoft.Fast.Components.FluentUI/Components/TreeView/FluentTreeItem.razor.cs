using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTreeItem : FluentComponentBase, IDisposable
{
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
    /// Gets or sets a callback that is triggered whenever <see cref="Expanded"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// When true, the control will appear selected by user interaction.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets a callback that is triggered whenever <see cref="Selected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }


    /// <summary>
    /// When true, the control will be immutable by user interaction. See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/disabled">disabled</see> HTML attribute for more information.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public FluentTreeItem()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    internal void SetSelected(bool value)
    {
        Selected = value;
    }

    internal void SetExpanded(bool value)
    {
        Expanded = value;
    }

    void IDisposable.Dispose() => Owner?.Unregister(this);

    private async Task HandleExpandedChangedAsync(TreeChangeEventArgs args)
    {
        if (args.AffectedId != Id)
            return;

        if (args.Expanded is bool expanded && expanded != Expanded && ExpandedChanged.HasDelegate)
        {
            Expanded = expanded;
            await ExpandedChanged.InvokeAsync(expanded);
        }
    }

    private async Task HandleSelectedChangedAsync(TreeChangeEventArgs args)
    {
        if (args.AffectedId != Id)
            return;

        if (args.Selected is bool selected && selected != Selected && SelectedChanged.HasDelegate)
        {
            Selected = selected;
            await SelectedChanged.InvokeAsync(selected);
        }
    }

}
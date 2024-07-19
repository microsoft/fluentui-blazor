using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTreeItem : FluentComponentBase, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTreeItem"/> class.
    /// </summary>
    public FluentTreeItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the list of sub-items to bind to the tree item
    /// </summary>
    [Parameter]
    public IEnumerable<ITreeViewItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the text of the tree item
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is expanded,
    /// and <see langword="false"/> if collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Called whenever <see cref="Expanded"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// When true, the control will appear selected by user interaction.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Called whenever <see cref="Selected"/> changes.
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

    /// <summary>
    /// If set to <see langword="true"/> then the tree item will
    /// be expanded when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <summary>
    /// If set to <see langword="true"/> then the tree item will
    /// be selected when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallySelected { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is collapsed.
    /// If this icon is not set, the <see cref="IconExpanded"/> will be used.
    /// </summary>
    [Parameter]
    public Icon? IconCollapsed { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of tree item,
    /// when the node is expanded.
    /// If this icon is not set, the <see cref="IconCollapsed"/> will be used.
    /// </summary>
    [Parameter]
    public Icon? IconExpanded { get; set; }

    /// <summary>
    /// Gets or sets the owning FluentTreeView
    /// </summary>
    [CascadingParameter]
    private FluentTreeView? Owner { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is collapsed,
    /// and <see langword="false"/> if expanded.
    /// </summary>
    public bool Collapsed => !Expanded;

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Owner?.Unregister(this);
        }

        _disposed = true;
    }

    internal async Task SetSelectedAsync(bool value)
    {
        if (value == Selected)
        {
            return;
        }

        Selected = value;

        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(Selected);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Owner?.Register(this);

        if (InitiallyExpanded && !Expanded)
        {
            Expanded = true;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(true);
            }
        }

        if (InitiallySelected)
        {
            await SetSelectedAsync(true);
        }
    }

    internal async Task HandleExpandedChangeAsync(TreeChangeEventArgs args)
    {
        if (args.AffectedId != Id || args.Expanded is null || args.Expanded == Expanded)
        {
            return;
        }

        Expanded = args.Expanded.Value;

        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(Expanded);
        }

        if (Owner != null)
        {
            await Owner.ItemExpandedChangeAsync(this);
        }
    }

    internal async Task HandleSelectedChangeAsync(TreeChangeEventArgs args)
    {
        if (args.AffectedId != Id || args.Selected is null || args.Selected == Selected)
        {
            return;
        }

        if (Owner?.Items == null)
        {
            await SetSelectedAsync(args.Selected.Value);
        }

        if (Owner != null)
        {
            await Owner.ItemSelectedChangeAsync(this);
        }
    }

    internal static RenderFragment GetFluentTreeItem(FluentTreeView owner, ITreeViewItem item)
    {
        RenderFragment fluentTreeItem = builder =>
        {
            int i = 0;
            builder.OpenComponent<FluentTreeItem>(i++);
            builder.AddAttribute(i++, "Id", item.Id);
            builder.AddAttribute(i++, "Items", item.Items);
            builder.AddAttribute(i++, "Text", item.Text);
            builder.AddAttribute(i++, "Selected", owner.SelectedItem == item);
            builder.AddAttribute(i++, "Expanded", item.Expanded);
            builder.AddAttribute(i++, "Disabled", item.Disabled);
            builder.AddAttribute(i++, "IconCollapsed", item.IconCollapsed);
            builder.AddAttribute(i++, "IconExpanded", item.IconExpanded);
            builder.SetKey(item.Id);

            if (owner.ItemTemplate != null)
            {
                builder.AddAttribute(i++, "ChildContent", owner.ItemTemplate(item));
            }

            builder.CloseComponent();
        };

        return fluentTreeItem;
    }
}

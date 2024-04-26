using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a selected checkbox updated when the user click on a row.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public class SelectColumn<TGridItem> : ColumnBase<TGridItem>
{
    public SelectColumn()
    {
        RenderFragment<TGridItem> DefaultChildContent = (context) => new RenderFragment((builder) =>
        {
            var selected = Property.Invoke(context);

            builder.OpenComponent<FluentIcon<Icon>>(0);
            builder.AddAttribute(1, "Value", selected ? IconChecked : IconUnchecked);
            builder.AddAttribute(2, "row-selected", selected);
            builder.CloseComponent();
        });

        Width = "50px";
        ChildContent = DefaultChildContent;
    }

    /// <summary>
    /// Gets or sets the content to be rendered for each row in the table.
    /// </summary>
    [Parameter]
    public RenderFragment<TGridItem> ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is non selected.
    /// </summary>
    [Parameter]
    public required Icon IconUnchecked { get; set; } = new CoreIcons.Regular.Size20.CheckboxUnchecked().WithColor(Color.FillInverse);

    /// <summary>
    /// Gets or sets the Icon to be rendered when the row is selected.
    /// </summary>
    [Parameter]
    public required Icon IconChecked { get; set; } = new CoreIcons.Filled.Size20.CheckboxChecked();

    /// <summary>
    /// Gets or sets the action to be executed when the row is selected or unselected.
    /// This action is required to update you data model.
    /// </summary>
    [Parameter]
    public EventCallback<TGridItem> OnSelect { get; set; }

    [Parameter]
    public bool? SelectAll { get; set; } = false;

    /// <summary>
    /// Gets or sets the action to be executed when the [All] checkbox is clicked.
    /// When this action is defined, the [All] checkbox is displayed.
    /// This action is required to update you data model.
    /// </summary>
    [Parameter]
    public EventCallback<bool?> SelectAllChanged { get; set; }

    /// <summary>
    /// Gets or sets the function to be executed to display the checked/unchecked icon, depending of you data model.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool> Property { get; set; } = (item) => false;

    /// <inheritdoc />
    [Parameter]
    public override GridSort<TGridItem>? SortBy { get; set; }

    /// <summary />
    internal async Task InverseSelectionAsync(TGridItem item)
    {
        SelectAll = null;
        await OnSelect.InvokeAsync(item);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (SelectAllChanged.HasDelegate)
        {
            HeaderContent = new RenderFragment((builder) =>
            {
                builder.OpenComponent<FluentIcon<Icon>>(0);
                builder.AddAttribute(1, "Value", SelectAll == true ? IconChecked : IconUnchecked);
                builder.AddAttribute(2, "all-selected", SelectAll);
                builder.AddAttribute(3, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAllAsync));
                builder.CloseComponent();
            });
        }
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    protected internal override string? RawCellContent(TGridItem item)
    {
        return TooltipText?.Invoke(item);
    }

    /// <inheritdoc />
    protected override bool IsSortableByDefault() => SortBy is not null;

    private async Task OnClickAllAsync(MouseEventArgs e)
    {
        if (Grid.Items == null)
        {
            return;
        }

        SelectAll = !(SelectAll == true);
        if (SelectAllChanged.HasDelegate)
        {
            await SelectAllChanged.InvokeAsync(SelectAll);
        }
    }
}

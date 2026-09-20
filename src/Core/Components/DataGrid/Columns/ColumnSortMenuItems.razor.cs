// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Renders the multi-column sort actions for one <see cref="FluentDataGrid{TGridItem}"/> column as menu items.
/// Used by the column header menu and by the column header popup, so that both offer the same actions.
/// </summary>
/// <typeparam name="TGridItem">The type of the data items displayed in the grid.</typeparam>
public partial class ColumnSortMenuItems<TGridItem>
{
    private static readonly string[] KEYBOARD_MENU_SELECT_KEYS = ["Enter", "NumpadEnter"];

    /// <summary />
    [Inject]
    public IFluentLocalizer Localizer { get; set; } = default!;

    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets the column these actions apply to.
    /// </summary>
    [Parameter, EditorRequired]
    public ColumnBase<TGridItem> Column { get; set; } = default!;

    /// <summary>
    /// Called after an action has been applied, so the menu or popup holding these items can close.
    /// </summary>
    [Parameter]
    public EventCallback OnActionApplied { get; set; }

    /// <summary>
    /// Gets or sets whether the first item takes focus when these items are rendered. Set by the column header popup,
    /// which has nothing else to move focus into it; the header menu focuses its own items.
    /// </summary>
    [Parameter]
    public bool AutofocusFirstItem { get; set; }

    private FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    private (int Level, bool Ascending)? SortLevel => Grid.GetSortLevel(Column);

    /// <summary>
    /// Gets whether ascending is offered. The direction the column is already sorted in is left out, so that every
    /// item on offer changes something.
    /// </summary>
    private bool ShowSortAscending => SortLevel?.Ascending != true;

    /// <summary>
    /// Gets whether descending is offered.
    /// </summary>
    private bool ShowSortDescending => SortLevel?.Ascending != false;

    private string? AutofocusAttribute(bool first)
        => AutofocusFirstItem && first ? "true" : null;

    /// <summary>
    /// Sorts by this column. A column that is already sorted on only changes direction and keeps its sort level;
    /// any other column becomes the column the grid is sorted by.
    /// </summary>
    private Task SortAsync(DataGridSortDirection direction)
        => ApplyAsync(SortLevel is null
            ? () => Grid.SortByColumnAsync(Column, direction)
            : () => Grid.AddSortByColumnAsync(Column, direction));

    private Task AddToSortAsync()
        => ApplyAsync(() => Grid.AddSortByColumnAsync(Column, DataGridSortDirection.Ascending));

    private Task ClearSortAsync()
        => ApplyAsync(() => Grid.RemoveSortByColumnAsync(Column));

    private Task ResetAllSortsAsync()
        => ApplyAsync(Grid.ResetSortAsync);

    private Task ClearAllSortsAsync()
        => ApplyAsync(Grid.ClearSortAsync);

    private async Task ApplyAsync(Func<Task> action)
    {
        await action();

        if (OnActionApplied.HasDelegate)
        {
            await OnActionApplied.InvokeAsync();
        }
    }

    /// <summary>
    /// Activates an item from the keyboard. A menu item raises its click event through the web component, which a
    /// keyboard activation does not go through.
    /// </summary>
    private static async Task HandleKeyDownAsync(KeyboardEventArgs args, Func<Task> action)
    {
        if (KEYBOARD_MENU_SELECT_KEYS.Contains(args.Key, StringComparer.OrdinalIgnoreCase))
        {
            await action();
        }
    }
}

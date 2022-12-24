using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using System;
using System.Buffers;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Microsoft.Fast.Components.FluentUI;

partial class ColumnOptions<TGridItem, TValue> : ColumnOptionsBase<TGridItem> 
{

    [CascadingParameter]
    private InternalGridContext<TGridItem> Owner { get; set; } = default!;

    [CascadingParameter]
    private ColumnBase<TGridItem> OwnerColumn { get; set; } = default!;

    private void openPopupClicked()
    {
        if (!dropDownIsOpen)
        {
            dropDownIsOpen = true;
            Owner.Grid.ShowColumnOptions((OwnerColumn as ColumnBase<TGridItem>)!);
        }
        else
            Owner.Grid.CloseColumnOptions();
    }

    private void getPropertyType()
    {
        if (OwnerColumn is null)
            return;
    }

    private void RemoveSort()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(OwnerColumn.GetType()))
        {
            var ccol = (ISortableColumn<TGridItem>)OwnerColumn;
            if (ccol is not null)
            {
                ccol.SortDirection = null;
                ccol.SortOrder = 0;
                Owner.Grid.ApplySort(ccol);
                Owner.Grid.CloseColumnOptions();
            }
        }
    }

    private void ApplySort()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(OwnerColumn.GetType()))
        {
            var ccol = (ISortableColumn<TGridItem>)OwnerColumn;
            if (ccol is not null)
            {
                ccol.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                Owner.Grid.ApplySort(ccol);
                Owner.Grid.CloseColumnOptions();
            }
        }
    }

    private void ApplySortDescending()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(OwnerColumn.GetType()))
        {
            var ccol = (ISortableColumn<TGridItem>)OwnerColumn;
            if (ccol is not null)
            {
                ccol.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                Owner.Grid.ApplySort(ccol);
                Owner.Grid.CloseColumnOptions();
            }
        }
    }

}

public abstract class ColumnOptionsBase<TGridItem> : ComponentBase
{
    protected bool dropDownIsOpen { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal void CloseDropDown()
    {
        dropDownIsOpen = false;
        StateHasChanged();
    }

}

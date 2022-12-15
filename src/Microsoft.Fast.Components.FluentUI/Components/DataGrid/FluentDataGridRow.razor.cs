using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#pragma warning disable Il2026

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGridRow<TGridItem> : FluentComponentBase, IHandleEvent, IDisposable where TGridItem : class
{

    #region Fields

    internal string RowId { get; } = Identifier.NewId();
    private readonly Dictionary<string, FluentDataGridCell<TGridItem>> cells = new();

    private string? _style = null;
    FluentDataGridCell<TGridItem>? _currentEditCell;

    Nullable<bool> _implementedIEditableObject;
    List<ValidationResult>? _validationResult;
    ValidationContext? _validationContext;

    #endregion

    #region Initialization

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    protected override void OnParametersSet()
    {
        if (Owner.Grid.Virtualize && RowType == DataGridRowType.Default)
        {
            _style = $"height: {Owner.Grid.RowsDataSize}px";
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the index of this row
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    /// Gets or sets the Item binded to row
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    /// String that gets applied to the the css gridTemplateColumns attribute for the row
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the type of row. See <see cref="DataGridRowType"/>
    /// </summary>
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TGridItem}"/> component
    /// </summary>
    [CascadingParameter]
    private InternalGridContext<TGridItem> Owner { get; set; } = default!;

    [Parameter]
    public IEnumerable<ColumnBase<TGridItem>>? Columns { get; set; }

    private DataGridRowOptions<TGridItem>? _Options;
    [Parameter]
    public DataGridRowOptions<TGridItem>? Options
    {
        get
        { return _Options; }
        set
        {
            _Options = value;
            if (_Options is not null)
            {
                RowIndex = _Options.RowIndex;
                Item = _Options.Item;
                GridTemplateColumns = _Options.GridTemplateColumns;
                Columns = _Options.Columns;
            }
        }
    }

    internal DataGridItemMode Mode { get; set; } = DataGridItemMode.Readonly;

    #endregion

    public void Dispose() => Owner.Unregister(this);

    internal void Register(FluentDataGridCell<TGridItem> cell)
    {
        if (!cells.Any(a => a.Value.GridColumn == cell.GridColumn))
            cells.Add(cell.CellId, cell);
        else
            cell.Dispose();
    }

    internal void Unregister(FluentDataGridCell<TGridItem> cell)
    {
        cells.Remove(cell.CellId);
    }

    #region Edit functions

    private async Task HandleOnCellFocus(DataGridCellFocusEventArgs args)
    {
        string? cellId = args.CellId;
        if (cells.TryGetValue(cellId!, out FluentDataGridCell<TGridItem>? cell))
        {
            await OnCellFocus.InvokeAsync(cell);
            if (cell is not null && Mode == DataGridItemMode.Edit && Columns is not null && Item is not null)
            {
                var col = this.Columns.ElementAt(cell.GridColumn - 1);
                if (_currentEditCell is not null && !cell.CellId.Equals(_currentEditCell.CellId) && CommitCell())
                {
                    _currentEditCell = cell;
                    ColBeginEdit(col);
                    col.SetFocuse();
                }
                else if (_currentEditCell is null)
                {
                    _currentEditCell = cell;
                    ColBeginEdit(col);
                    col.SetFocuse();
                }
            }
        }
    }
   
    internal bool BeginEdit()
    {
        if (!_implementedIEditableObject.HasValue)
            _implementedIEditableObject = typeof(IEditableObject).IsAssignableFrom(typeof(TGridItem));
        if (Columns is null || Item is null)
            return false;
        if (Mode == DataGridItemMode.Readonly && Owner.Grid.BeginItemEdit(this))
        {
            _validationContext = new ValidationContext(Item);
            _validationResult = new List<ValidationResult>();
            Mode = DataGridItemMode.Edit;
            foreach (var col in Columns.Where(w => typeof(IBindColumn).IsAssignableFrom(w.GetType())))
            {
                ColBeginEdit(col);
            }
            Columns.First().SetFocuse();
            Owner.Grid.setCellEditableConfig(this);
        }
        else
            return false;
        return true;
    }

    internal void EndEdit()
    {
        if (_implementedIEditableObject!.Value)
            ((IEditableObject)Item!).EndEdit();
        Mode = DataGridItemMode.Readonly;
        _currentEditCell = null;
        StateHasChanged();
        Owner.Grid.removeCellEditableConfig(this);
    }

    internal async Task<bool> CanCommit()
    {
        if (Columns is null || Item is null)
            return true;
        if (Mode == DataGridItemMode.Readonly)
            return false;
        await Task.Delay(10);
        if (!CommitCell())
            return false;
        CommitEdit();
        _validationContext!.MemberName = null;
        return Validator.TryValidateObject(Item!, _validationContext!, _validationResult!);
    }

    private bool CommitCell()
    {
        if (_currentEditCell is null)
            return true;
        var col = Columns!.ElementAt(_currentEditCell.GridColumn - 1);
        if (!col.IsEditable)
            return true;
        var cprvcol = ((IBindColumn)col);
        if (cprvcol is not null && cprvcol.PropertyInfo.Name is not null)
        {
            _validationContext.MemberName = cprvcol.PropertyInfo.Name;
            //if property does not have valide data or event has Cancel == true, prevent cell change and set focuse to previouse cell
            if (!Validator.TryValidateProperty(col.GetCurrentValue(), _validationContext, _validationResult)
                || !Owner.Grid.EditEndingForCell(Item!, cprvcol.PropertyInfo.Name, col.GetCurrentValue(), EditActionEnum.Commit))
            {
                col.SetFocuse();
                return false;
            }
            else
            {
                col.UpdateSource();
                Owner.Grid.EditEndedForCell(Item!, cprvcol.PropertyInfo.Name, EditActionEnum.Commit);
            }

        }
        return true;
    }

    private void ColBeginEdit(ColumnBase<TGridItem> col)
    {
        col.BeginEdit(Item!);
        if (!col.IsReadonly)
            col.InternalIsReadonly = !Owner.Grid.BeginPropertyEdit(Item!, ((IBindColumn)col)!.PropertyInfo.Name!);
        else
            col.InternalIsReadonly = true;
    }

    private void CommitEdit()
    {
        foreach (var col in Columns!)
        {
            col.UpdateSource();
        }
        StateHasChanged();
    }

    internal async void CancelEdit()
    {
        if (_implementedIEditableObject!.Value)
            ((IEditableObject)Item!).CancelEdit();
        if (_currentEditCell is not null)
        {
            var col = Columns!.ElementAt(_currentEditCell.GridColumn - 1);
            if (typeof(IBindColumn).IsAssignableFrom(col.GetType()))
                Owner.Grid.EditEndedForCell(Item!, ((IBindColumn)col)!.PropertyInfo.Name!, EditActionEnum.Cancel);
            _currentEditCell = null;
        }
        if (Mode == DataGridItemMode.Edit)
            Mode = DataGridItemMode.Readonly;
        await Owner.Grid.EndEdit(this, EditActionEnum.Cancel);
        Owner.Grid.removeCellEditableConfig(this);
        StateHasChanged();
    }

    private void OnRowDblClicked()
    {
        if (Owner.Grid.IsReadonly)
            return;
        BeginEdit();
    }

    #endregion

    private static string? ColumnClass(ColumnBase<TGridItem> column) => column.Align switch
    {
        Align.Center => $"col-justify-center {column.Class}",
        Align.Right => $"col-justify-end {column.Class}",
        _ => column.Class,
    };


    Task IHandleEvent.HandleEventAsync(
       EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);
}
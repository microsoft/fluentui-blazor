using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataFilterProperty<TItem>
{
    /// <summary>
    /// Gets or sets the orientation of the stacked components. 
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;




    //private bool _show;
    //private const string IdPrefixFilter = "Filter-";
    //private readonly string _idMenu;

    //[Inject]
    //private IDialogService DialogService { get; set; } = default!;

    //public FluentDataFilterManager() => _idMenu = Identifier.NewId();

    ///// <summary>
    ///// Gets or sets items.
    ///// </summary>
    //[Parameter]
    //public IList<DataFilterManagerItem<TItem>> Items { get; set; } = [];

    ///// <summary>
    ///// Gets or sets items changed.
    ///// </summary>
    //[Parameter]
    //public EventCallback<IList<DataFilterManagerItem<TItem>>> ItemsChanged { get; set; } = default!;

    ///// <summary>
    ///// Gets or sets criteria.
    ///// </summary>
    //[Parameter]
    //public DataFilterCriteria<TItem> Criteria { get; set; } = new();

    ///// <summary>
    ///// Gets or sets Criteria changed
    ///// </summary>
    //[Parameter]
    //public EventCallback<DataFilterCriteria<TItem>> CriteriaChanged { get; set; } = default!;

    ///// <summary>
    ///// Gets or sets the selected item.
    ///// </summary>
    //[Parameter]
    //public virtual DataFilterManagerItem<TItem>? SelectedItem { get; set; }

    ///// <summary>
    ///// Called whenever the selection changed.
    ///// </summary>
    //[Parameter]
    //public virtual EventCallback<DataFilterManagerItem<TItem>?> SelectedItemChanged { get; set; }

    ///// <summary>
    ///// Gets or sets text button menu.
    ///// </summary>
    //[Parameter]
    //public string TextButtonMenu { get; set; } = "Filter";

    ///// <summary>
    ///// Gets or sets text new filter.
    ///// </summary>
    //[Parameter]
    //public string TextNewFilter { get; set; } = "New";

    ///// <summary>
    ///// Gets or sets text clear all filters.
    ///// </summary>
    //[Parameter]
    //public string TextClear { get; set; } = "Clear";

    ///// <summary>
    ///// Gets or sets text edit filter.
    ///// </summary>
    //[Parameter]
    //public string TextEditFilter { get; set; } = "Edit";

    ///// <summary>
    ///// Gets or sets text panel save.
    ///// </summary>
    //[Parameter]
    //public string TextPanelSave { get; set; } = "Save";

    ///// <summary>
    ///// Gets or sets text panel delete.
    ///// </summary>
    //[Parameter]
    //public string TextPanelDelete { get; set; } = "Delete";

    ///// <summary>
    ///// Gets or sets text panel cancel.
    ///// </summary>
    //[Parameter]
    //public string TextPanelCancel { get; set; } = "Cancel";

    ///// <summary>
    ///// Gets or sets text panel name.
    ///// </summary>
    //[Parameter]
    //public string TextPanelName { get; set; } = "Name";

    ///// <summary>
    ///// Gets or sets text panel reset previous.
    ///// </summary>
    //[Parameter]
    //public string TextPanelResetPrevious { get; set; } = "Reset previous";

    ///// <summary>
    ///// Gets or sets text panel export.
    ///// </summary>
    //[Parameter]
    //public string TextPanelExport { get; set; } = "Export";

    ///// <summary>
    ///// Gets or sets text panel import.
    ///// </summary>
    //[Parameter]
    //public string TextPanelImport { get; set; } = "Import";

    ///// <summary>
    ///// Gets or sets text panel tile new filter.
    ///// </summary>
    //[Parameter]
    //public string TextPanelTitleNewFilter { get; set; } = "New filter";

    ///// <summary>
    ///// Gets or sets text panel tile edit filter.
    ///// </summary>
    //[Parameter]
    //public string TextPanelTitleEditFilter { get; set; } = "Edit filter";

    ///// <summary>
    ///// Gets or sets the width of the panel.
    ///// </summary>
    //[Parameter]
    //public string? PanelWidth { get; set; } = "700px";

    ///// <summary>
    ///// Gets or sets allow import json criteria of the panel.
    ///// </summary>
    //[Parameter]
    //public bool AllowPanelImport { get; set; }

    ///// <summary>
    ///// Gets or sets allow export json criteria of the panel.
    ///// </summary>
    //[Parameter]
    //public bool AllowPanelExport { get; set; }

    ///// <summary>
    ///// Gets or sets DataFilter Template.
    ///// </summary>
    //[Parameter, EditorRequired]
    //public RenderFragment<DataFilterCriteria<TItem>> DataFilterTemplate { get; set; } = default!;

    ///// <summary>
    ///// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    ///// Defaults to <seealso cref="AspNetCore.Components.Appearance.Neutral"/>
    ///// </summary>
    //[Parameter]
    //public Appearance? Appearance { get; set; } = AspNetCore.Components.Appearance.Neutral;

    //private void ShowMenu() => _show = !_show;

    //private DataFilterManagerDialogContext<TItem> CreateDialogContext(string title, bool allowDelete)
    //    => new()
    //    {
    //        Name = title,
    //        AllowDelete = allowDelete,
    //        FilterManager = this,
    //    };

    //private async Task OnMenuChangeAsync(MenuChangeEventArgs e)
    //{
    //    _show = false;

    //    var id = e.Id + "";
    //    if (id == "New")
    //    {
    //        var oldCriteria = Criteria;
    //        Criteria = new();
    //        await CriteriaChangedAsync();

    //        var data = CreateDialogContext(string.Empty, false);
    //        var result = await ShowDialogAsync(TextPanelTitleNewFilter, data);
    //        if (!result.Cancelled && !Criteria.IsEmpty)
    //        {
    //            var item = new DataFilterManagerItem<TItem>()
    //            {
    //                AllowEdit = true,
    //                Criteria = Criteria,
    //                Title = data.Name,
    //            };

    //            Items.Add(item);
    //            await SetCurrentItemAsync(item);
    //            await ItemsChangedAsync();
    //        }
    //        else
    //        {
    //            Criteria = oldCriteria;
    //            await CriteriaChangedAsync();
    //        }
    //    }
    //    else if (id == "Edit")
    //    {
    //        var oldCriteria = Criteria.Clone();

    //        var data = CreateDialogContext(SelectedItem!.Title, true);
    //        var result = await ShowDialogAsync(TextPanelTitleEditFilter, data);
    //        if (!result.Cancelled)
    //        {
    //            if (data.IsDeleted)
    //            {
    //                Items.Remove(SelectedItem);
    //                await SetCurrentItemAsync(null!);
    //                await ItemsChangedAsync();
    //            }
    //            else if (!Criteria.IsEmpty)
    //            {
    //                await CriteriaChangedAsync();
    //            }
    //            else
    //            {
    //                SelectedItem.Criteria = oldCriteria;
    //                await CriteriaChangedAsync();
    //            }
    //        }
    //        else
    //        {
    //            SelectedItem.Criteria = oldCriteria;
    //            await CriteriaChangedAsync();
    //        }
    //    }
    //    else if (id == "Clear")
    //    {
    //        await SetCurrentItemAsync(null!);
    //    }
    //    else if (id.StartsWith(IdPrefixFilter))
    //    {
    //        await SetCurrentItemAsync(ItemsOrdered[int.Parse(id[IdPrefixFilter.Length..])]);
    //    }
    //}

    //private async Task SetCurrentItemAsync(DataFilterManagerItem<TItem> item)
    //{
    //    SelectedItem = item;
    //    Criteria = SelectedItem?.Criteria ?? new();

    //    await SelectedItemChangedAsync();
    //    await CriteriaChangedAsync();
    //}

    //private IList<DataFilterManagerItem<TItem>> ItemsOrdered => Items.OrderBy(a => a.Title).ToList();

    //private async Task<DialogResult> ShowDialogAsync(string title, DataFilterManagerDialogContext<TItem> data)
    //{
    //    var options = new DialogParameters<DataFilterManagerDialogContext<TItem>>()
    //    {
    //        Content = data,
    //        Title = title,
    //        Width = PanelWidth,
    //        PrimaryActionEnabled = false,
    //        SecondaryActionEnabled = false,
    //    };

    //    var _dialog = await DialogService.ShowPanelAsync<DataFilterManagerDialog<TItem>>(data, options);
    //    return await _dialog.Result;
    //}

    //internal async Task CriteriaChangedAsync()
    //{
    //    if (CriteriaChanged.HasDelegate)
    //    {
    //        await CriteriaChanged.InvokeAsync(Criteria);
    //    }
    //}

    //private async Task SelectedItemChangedAsync()
    //{
    //    if (SelectedItemChanged.HasDelegate)
    //    {
    //        await SelectedItemChanged.InvokeAsync(SelectedItem);
    //    }
    //}

    //private async Task ItemsChangedAsync()
    //{
    //    if (ItemsChanged.HasDelegate)
    //    {
    //        await ItemsChanged.InvokeAsync(Items);
    //    }
    //}
}

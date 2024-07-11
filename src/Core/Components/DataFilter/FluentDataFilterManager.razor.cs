using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataFilterManager<TItem>
{
    private bool _show;
    private const string IdPrefixFilter = "Filter-";
    private readonly string _idMenu;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    public FluentDataFilterManager() => _idMenu = Identifier.NewId();

    /// <summary>
    /// Gets or sets items.
    /// </summary>
    [Parameter]
    public IList<DataFilterManagerItem<TItem>> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets items changed.
    /// </summary>
    [Parameter]
    public EventCallback<IList<DataFilterManagerItem<TItem>>> ItemsChanged { get; set; } = default!;

    /// <summary>
    /// Gets or sets criteria.
    /// </summary>
    [Parameter]
    public DataFilterCriteria<TItem> Criteria { get; set; } = new();

    /// <summary>
    /// Gets or sets Criteria changed
    /// </summary>
    [Parameter]
    public EventCallback<DataFilterCriteria<TItem>> CriteriaChanged { get; set; } = default!;

    /// <summary>
    /// Gets or sets text button menu.
    /// </summary>
    [Parameter]
    public string TextButtonMenu { get; set; } = "Filter";

    /// <summary>
    /// Gets or sets text new filter.
    /// </summary>
    [Parameter]
    public string TextNewFilter { get; set; } = "New";

    /// <summary>
    /// Gets or sets text clear all filters.
    /// </summary>
    [Parameter]
    public string TextClearAllFilters { get; set; } = "Clear";

    /// <summary>
    /// Gets or sets text edit filter.
    /// </summary>
    [Parameter]
    public string TextEditFilter { get; set; } = "Edit";

    /// <summary>
    /// Gets or sets text panel save.
    /// </summary>
    [Parameter]
    public string TextPanelSave { get; set; } = "Save";

    /// <summary>
    /// Gets or sets text panel delete.
    /// </summary>
    [Parameter]
    public string TextPanelDelete { get; set; } = "Delete";

    /// <summary>
    /// Gets or sets text panel cancel.
    /// </summary>
    [Parameter]
    public string TextPanelCancel { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets text panel title.
    /// </summary>
    [Parameter]
    public string TextPanelTitle { get; set; } = "Title";

    /// <summary>
    /// Gets or sets the width of the panel.
    /// </summary>
    [Parameter]
    public string? PanelWidth { get; set; } = "700px";

    /// <summary>
    /// Gets or sets DataFilter Template.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<DataFilterCriteria<TItem>> DataFilterTemplate { get; set; } = default!;

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// Defaults to <seealso cref="AspNetCore.Components.Appearance.Neutral"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = AspNetCore.Components.Appearance.Neutral;

    private void ShowMenu() => _show = !_show;

    private DataFilterManagerDialogContext<TItem> CreateDialogContext(string title, bool allowDelete)
        => new()
        {
            Criteria = Criteria,
            Title = title,
            AllowDelete = allowDelete,
            DataFilterTemplate = DataFilterTemplate,
            TextCancel = TextPanelCancel,
            TextSave = TextPanelSave,
            TextDelete = TextPanelDelete,
            TextTitle = TextPanelTitle,
        };

    private async Task OnMenuChangeAsync(MenuChangeEventArgs e)
    {
        var id = e.Id + "";
        if (id == "New")
        {
            var oldCriteria = Criteria;
            Criteria = new();
            await CriteriaChangedAsync();
            var data = CreateDialogContext(string.Empty, false);
            var result = await ShowDialogAsync(TextNewFilter, data);
            if (!result.Cancelled && !data.Criteria.IsEmpty)
            {
                Items.Add(new()
                {
                    AllowEdit = true,
                    Criteria = data.Criteria,
                    Title = data.Title,
                });

                CurrentItem = Items.Last();
                await ItemsChangedAsync();
            }
            else
            {
                Criteria = oldCriteria;
            }
        }
        else if (id == "Edit")
        {
            var oldCriteria = Criteria.Clone();
            var currentItem = CurrentItem;

            var data = CreateDialogContext(currentItem!.Title, true);
            var result = await ShowDialogAsync(TextEditFilter, data);
            if (!result.Cancelled)
            {
                if (data.IsDeleted)
                {
                    Items.Remove(currentItem!);
                    CurrentItem = null;
                }
                else if (!data.Criteria.IsEmpty)
                {
                    currentItem!.Criteria = data.Criteria;
                }
                await ItemsChangedAsync();
            }
            else
            {
                currentItem.Criteria= oldCriteria;
                CurrentItem = currentItem;
            }
        }
        else if (id == "Clear")
        {
            CurrentItem = null;
        }
        else if (id.StartsWith(IdPrefixFilter))
        {
            Criteria = Items[int.Parse(id[IdPrefixFilter.Length..])].Criteria;
        }

        await CriteriaChangedAsync();
    }

    private DataFilterManagerItem<TItem>? CurrentItem
    {
        get => Items.FirstOrDefault(a => a.Criteria == Criteria);
        set
        {
            var idx = Items.IndexOf(value!);
            Criteria = idx == -1
                        ? new()
                        : Items[idx].Criteria;
        }
    }

    private async Task<DialogResult> ShowDialogAsync(string title, DataFilterManagerDialogContext<TItem> data)
    {
        var options = new DialogParameters<DataFilterManagerDialogContext<TItem>>()
        {
            Content = data,
            Title = title,
            Width = PanelWidth,
            PrimaryActionEnabled = false,
            SecondaryActionEnabled = false,
        };

        var _dialog = await DialogService.ShowPanelAsync<DataFilterManagerDialog<TItem>>(data, options);
        return await _dialog.Result;
    }

    private async Task CriteriaChangedAsync()
    {
        if (CriteriaChanged.HasDelegate)
        {
            await CriteriaChanged.InvokeAsync(Criteria);
        }
    }

    private async Task ItemsChangedAsync()
    {
        if (ItemsChanged.HasDelegate)
        {
            await ItemsChanged.InvokeAsync(Items);
        }
    }
}

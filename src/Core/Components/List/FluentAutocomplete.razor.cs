// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentAutocomplete allows for selecting one or more options from a list of options with autocomplete functionality.
/// </summary>
/// <typeparam name="TOption"></typeparam>
/// <typeparam name="TValue"></typeparam>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentAutocomplete<TOption, TValue> : FluentListBase<TOption, TValue>
{
    private static readonly Icon SearchIcon = new CoreIcons.Regular.Size20.Search();
    private static readonly Icon BadgeCloseIcon = new CoreIcons.Regular.Size20.Dismiss();

    private string? _textInput;
    private bool _isOpen;
    private bool _isRemovingOneItem;
    private bool _inProgress;

    // List of items used in the internally filtered listbox
    private List<TOption> _internalFilteredItems = [];
    private List<TOption> _internalSelectedItems = [];

    /// <summary />
    public FluentAutocomplete(LibraryConfiguration configuration) : base(configuration)
    {
        // Default values
        Id = Identifier.NewId();
        Multiple = true;
    }

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 400 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 200;

    /// <summary>
    /// Filter the list of options (items) using the text written by the user.
    /// </summary>
    [Parameter]
    public EventCallback<OptionsSearchEventArgs<TOption>> OnOptionsSearch { get; set; }

    /// <summary />
    public override IEnumerable<TOption> SelectedItems
    {
        get => _internalSelectedItems;
        set => _internalSelectedItems = [.. value];
    }

    /// <summary>
    /// Gets or sets the number of maximum options (items) returned by <see cref="OnOptionsSearch"/>.
    /// Default value is 9.
    /// </summary>
    [Parameter]
    public int MaximumOptionsSearch { get; set; } = 9;

    /// <summary>
    /// Gets or sets whether the component will display a progress indicator while fetching data.
    /// A progress ring will be shown at the end of the component, when the <see cref="OnOptionsSearch"/> is invoked.
    /// </summary>
    [Parameter]
    public bool ShowProgressIndicator { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Autocomplete.initialize", Id);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task InternalSelectedItemsChangedHandlerAsync(IEnumerable<TOption> items)
    {
        var itemsToAdd = items.Where(item => !_internalSelectedItems.Contains(item, EqualityComparer<TOption>.Default));
        var itemsToRemove = _internalFilteredItems.Where(item => !items.Contains(item, EqualityComparer<TOption>.Default)).ToList();

        // Add items that are in 'items' but not already in _internalSelectedItems
        _internalSelectedItems.AddRange(itemsToAdd);

        // Remove items that are in '_internalFilteredItems' but not in 'items' anymore
        foreach (var item in itemsToRemove)
        {
            _internalSelectedItems.Remove(item);
        }

        // Raise event
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
        }
    }

    /// <summary />
    private async Task OnTextInputChangedAsync()
    {
        _inProgress = true;

        var args = new OptionsSearchEventArgs<TOption>()
        {
            Items = [],
            Text = _textInput ?? string.Empty,
        };

        await OnOptionsSearch.InvokeAsync(args);

        _internalFilteredItems = [.. args.Items?.Take(MaximumOptionsSearch) ?? []];

        if (!_isRemovingOneItem)
        {
            _isOpen = true;
        }

        _inProgress = false;
        _isRemovingOneItem = false;
    }

    /// <summary />
    private async Task RemoveSelectedItemAsync(TOption? item)
    {
        if (item is null)
        {
            return;
        }

        _isOpen = false;
        _isRemovingOneItem = true;

        _internalSelectedItems.Remove(item);

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
        }
    }
}

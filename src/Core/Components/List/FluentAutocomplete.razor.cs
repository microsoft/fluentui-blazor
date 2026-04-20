// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
    private static readonly Icon ClearIcon = new CoreIcons.Regular.Size20.Dismiss();

    private string? _textInput;
    private bool _isOpen;
    private bool _inProgress;

    // List of items used in the internally filtered listbox
    private List<TOption> _internalFilteredItems = [];
    private List<TOption> _internalSelectedItems = [];
    private TOption? _internalSelectedItem => _internalSelectedItems.FirstOrDefault();

    /// <summary />
    public FluentAutocomplete(LibraryConfiguration configuration) : base(configuration)
    {
        // Default values
        Id = Identifier.NewId();

        // Set default value: if `Width` is not already set (not null),
        Width ??= "160px";

        // Set default value: if `Multiple` is not already set to `false` using `base(configuration)`, in the Program.cs
        // (not used since the Multiple is overridden with a default value of true directly in this class)
        // configuration?.DefaultValues.SetInitialValues(this, [(nameof(Multiple), true)]);
    }

    /// <summary />
    protected override string? StyleValue => new StyleBuilder(base.StyleValue)
        .AddStyle("--max-selected-width", MaxSelectedWidth)
        .Build();

    /// <summary>
    /// Gets or sets the appearance of the text input.
    /// Default is <see cref="TextInputAppearance.Outline"/>.
    /// </summary>
    [Parameter]
    public TextInputAppearance InputAppearance { get; set; } = TextInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets whether the list allows multiple selections.
    /// </summary>
    [Parameter]
    public override bool Multiple { get; set; } = true;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 400 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 400;

    /// <summary>
    /// Filter the list of options (items) using the text written by the user.
    /// </summary>
    [Parameter]
    public EventCallback<OptionsSearchEventArgs<TOption>> OnOptionsSearch { get; set; }

    /// <inheritdoc cref="FluentListBase{TOption, TValue}.SelectedItems" />
    public override IEnumerable<TOption> SelectedItems
    {
        get => _internalSelectedItems;
        set => _internalSelectedItems = Multiple ? [.. value] : [.. value.Take(1)];
    }

    /// <summary>
    /// Gets or sets the number of maximum options (items) returned by <see cref="OnOptionsSearch"/>.
    /// Default value is 9.
    /// </summary>
    [Parameter]
    public int MaximumOptionsSearch { get; set; } = 9;

    /// <summary>
    /// Gets or sets the maximum number of options (items) selected.
    /// Exceeding this value requires the user to delete some elements in order to select new ones.
    /// See the <see cref="MaximumSelectedOptionsMessage"/>.
    /// </summary>
    [Parameter]
    public int? MaximumSelectedOptions { get; set; }

    /// <summary>
    /// Gets or sets the message displayed when the <see cref="MaximumSelectedOptions"/> is reached.
    /// </summary>
    [Parameter]
    public RenderFragment? MaximumSelectedOptionsMessage { get; set; }

    /// <summary>
    /// Gets or sets whether the component will display a progress indicator while fetching data.
    /// A progress ring will be shown at the end of the component, when the <see cref="OnOptionsSearch"/> is invoked.
    /// You can customize the progress indicator by using the <see cref="HeaderContent"/> or <see cref="FooterContent"/> parameters: see <see cref="AutocompleteHeaderFooterContent{TOption}.InProgress"/>.
    /// </summary>
    [Parameter]
    public bool ShowProgressIndicator { get; set; }

    /// <summary>
    /// Gets or sets the maximum height of the selected items panel. A common value is 'unset' (unlimited) or '200px'.
    /// If this parameter is not set, all selected items will be shown on a single line.
    /// </summary>
    [Parameter]
    public string? MaxAutoHeight { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of the selected items.
    /// </summary>
    [Parameter]
    public string? MaxSelectedWidth { get; set; }

    /// <summary>
    /// Gets or sets whether the Search icon or Clear button is displayed.
    /// </summary>
    [Parameter]
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Gets or sets the icon used for the Clear button. By default: Dismiss icon.
    /// </summary>
    [Parameter]
    public Icon IconDismiss { get; set; } = ClearIcon;

    /// <summary>
    /// Gets or sets the icon used for the Search button. By default: Search icon.
    /// </summary>
    [Parameter]
    public Icon IconSearch { get; set; } = SearchIcon;

    /// <summary>
    /// Gets or sets the template for the selected options, displayed in the autocomplete input text.
    /// </summary>
    [Parameter]
    public RenderFragment<TOption>? SelectedOptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the header content, placed at the top of the popup panel.
    /// </summary>
    [Parameter]
    public RenderFragment<AutocompleteHeaderFooterContent<TOption>>? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the footer content, placed at the bottom of the popup panel.
    /// </summary>
    [Parameter]
    public RenderFragment<AutocompleteHeaderFooterContent<TOption>>? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets the currently selected option, when <see cref="FluentListBase{TOption, TValue}.Multiple"/> is false.
    /// </summary>
    [Parameter]
    public TOption? SelectedItem { get; set; }

    /// <summary>
    /// Gets or sets an event callback that is raised when the <see cref="SelectedItem"/> changes. 
    /// This is only relevant when <see cref="FluentListBase{TOption, TValue}.Multiple"/> is false.
    /// </summary>
    [Parameter]
    public EventCallback<TOption> SelectedItemChanged { get; set; }

    /// <summary>
    /// Gets a value indicating whether the number of selected options has reached the maximum defined by <see cref="MaximumSelectedOptions"/>.
    /// </summary>
    public bool IsReachedMaxItems => MaximumSelectedOptions.HasValue && _internalSelectedItems.Count >= MaximumSelectedOptions.Value;

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

    /// <summary />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        // Check if SelectedItem is being supplied and has changed
        if (parameters.TryGetValue<TOption?>(nameof(SelectedItem), out var newSelectedItem))
        {
            var comparer = OptionSelectedComparer ?? EqualityComparer<TOption>.Default;
            var currentSelectedItem = _internalSelectedItem;

            if (!comparer.Equals(newSelectedItem, currentSelectedItem))
            {
                // Sync _internalSelectedItems with the new value
                _internalSelectedItems = newSelectedItem is not null ? [newSelectedItem] : [];
                SelectedItem = newSelectedItem;
            }
        }

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Raised when the FluentListbox.SelectedItems property changes.
    /// </summary>
    private async Task InternalSelectedItemsChangedHandlerAsync(IEnumerable<TOption> items)
    {
        var comparer = OptionSelectedComparer ?? EqualityComparer<TOption>.Default;
        var itemsToAdd = items.Where(item => !_internalSelectedItems.Contains(item, comparer)).ToList();
        var itemsToRemove = _internalFilteredItems.Where(item => !items.Contains(item, comparer)).ToList();

        // Multiple = True
        if (Multiple)
        {
            // Add items that are in 'items' but not already in _internalSelectedItems
            _internalSelectedItems.AddRange(itemsToAdd);

            // Remove items that are in '_internalFilteredItems' but not in 'items' anymore
            foreach (var item in itemsToRemove)
            {
                _internalSelectedItems.RemoveAll(selectedItem => comparer.Equals(selectedItem, item));
            }
        }

        // Multiple = False
        else
        {
            var selectedItem = _internalSelectedItems.FirstOrDefault();
            var isInsideFilteredItems = _internalFilteredItems.Any(item => comparer.Equals(item, selectedItem));
            if (!items.Any() && isInsideFilteredItems)
            {
                _internalSelectedItems.Clear();
            }
            else
            {
                var singleItemToAdd = itemsToAdd.FirstOrDefault();
                if (singleItemToAdd != null)
                {
                    _internalSelectedItems.Clear();
                    _internalSelectedItems.Add(singleItemToAdd);
                }
            }
        }

        SelectedItem = _internalSelectedItem;

        // Raise event
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
        }

        if (SelectedItemChanged.HasDelegate)
        {
            await SelectedItemChanged.InvokeAsync(_internalSelectedItem);
        }

        if (ValueChanged.HasDelegate)
        {
            var value = GetOptionValue(_internalSelectedItem);
            await ValueChanged.InvokeAsync(value);
        }

        await SetInputFocusAsync();
    }

    /// <summary>
    /// Detect when the user presses 'Backspace' or 'ArrowDown' keys in the text input.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private async Task OnTextInputKeyDownAsync(KeyboardEventArgs args)
    {
        switch (args.Key)
        {
            // When Backspace is pressed and there is no text in the input, remove the last selected item
            case "Backspace":
            case "Delete":
                if (string.IsNullOrEmpty(_textInput) && _internalSelectedItems.Any())
                {
                    await RemoveSelectedItemAsync(_internalSelectedItems.Last());
                }

                break;

            // When ArrowDown is pressed and the listbox is closed, open it
            // If there are no yet any items in the list, it means the user hasn't typed anything, so we can open the listbox and show all the options
            case "ArrowDown":
                if (!_isOpen)
                {
                    await DisplayFilteredOptionsAsync(showWhenInputIsEmpty: true);
                }

                break;

            case "Enter":
                // WARN: The option selection feature is done using JS code (FluentAutocomplete.ts)

                // If not yet open, do the same as pressing ArrowDown.
                if (!_isOpen)
                {
                    await OnTextInputKeyDownAsync(new KeyboardEventArgs { Key = "ArrowDown" });
                }

                // If already open, close the listbox and let the JS code handle the rest of the logic for selecting the option.
                else
                {
                    _isOpen = false;

                    if (!string.IsNullOrEmpty(_textInput))
                    {
                        _textInput = string.Empty;
                        if (ValueChanged.HasDelegate)
                        {
                            await ValueChanged.InvokeAsync((TValue)(object)_textInput);
                        }
                    }
                }

                break;
        }
    }

    /// <summary>
    /// When the user types in the input, display the listbox with the filtered options.
    /// </summary>
    /// <returns></returns>
    internal async Task DisplayFilteredOptionsAsync(bool showWhenInputIsEmpty)
    {
        // Raise the ValueChanged event to notify the parent component.
        if (ValueChanged.HasDelegate)
        {
            var value = _textInput ?? string.Empty;
            await ValueChanged.InvokeAsync((TValue)(object)value);
        }

        // If the input is empty, we don't show any options in the listbox, and we close it if it was open
        if (!showWhenInputIsEmpty && string.IsNullOrEmpty(_textInput))
        {
            _isOpen = false;
            StateHasChanged();
            return;
        }

        _inProgress = true;
        _isOpen = true;

        StateHasChanged();

        // Raise the OnOptionsSearch event to get the filtered list of items.
        if (OnOptionsSearch.HasDelegate)
        {
            var args = new OptionsSearchEventArgs<TOption>()
            {
                Items = [],
                Text = _textInput ?? string.Empty,
            };

            await OnOptionsSearch.InvokeAsync(args);

            _internalFilteredItems = [.. args.Items?.Take(MaximumOptionsSearch) ?? []];
        }

        // Use the Items parameter to filter the list of items
        else if (Items != null)
        {
            _internalFilteredItems = [.. Items.Where(item => GetOptionText(item)?.StartsWith(_textInput ?? string.Empty, StringComparison.InvariantCultureIgnoreCase) == true).Take(MaximumOptionsSearch)];
        }

        // No source of items provided
        else
        {
            _internalFilteredItems = [];
        }

        _inProgress = false;
    }

    /// <summary />
    private Task DisplayFilteredOptionsAsync() => DisplayFilteredOptionsAsync(showWhenInputIsEmpty: true);

    /// <summary>
    /// When the user clicks the "x" button or presses Backspace with an empty input, remove the selected or latest item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    internal async Task RemoveSelectedItemAsync(TOption? item)
    {
        if (item is null)
        {
            return;
        }

        _isOpen = false;
        _internalSelectedItems.Remove(item);

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
        }

        if (SelectedItemChanged.HasDelegate)
        {
            await SelectedItemChanged.InvokeAsync(_internalSelectedItem);
        }
    }

    /// <summary>
    /// When the user clicks the search icon, open or close the listbox with the filtered options depending on its current state.
    /// </summary>
    private async Task SwitchOptionsPopupAsync()
    {
        if (_isOpen)
        {
            _isOpen = false;
        }
        else
        {
            await DisplayFilteredOptionsAsync(showWhenInputIsEmpty: true);
        }
    }

    /// <summary>
    /// When the user clicks the "x" button to clear the selection, remove all selected items and close the listbox.
    /// </summary>
    /// <returns></returns>
    private async Task ClearSelectionAsync()
    {
        _isOpen = false;

        if (Multiple)
        {
            _internalSelectedItems.Clear();

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
            }
        }
        else
        {
            SelectedItem = default;
            if (SelectedItemChanged.HasDelegate)
            {
                await SelectedItemChanged.InvokeAsync(SelectedItem);
            }
        }
    }

    private async Task OnOptionsPopupClosedAsync()
    {
        // After closing the popup
        if (!_isOpen)
        {
            await SetInputFocusAsync();
        }
    }

    /// <summary>
    /// Sets the focus to the text input element.
    /// </summary>
    private async Task SetInputFocusAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Autocomplete.setFocus", Id);
    }
}

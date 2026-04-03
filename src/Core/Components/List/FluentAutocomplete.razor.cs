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
        Multiple = true;
        Width = "160px";
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
                _internalSelectedItems.Remove(item);
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

        // Raise event
        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
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
    private async Task DisplayFilteredOptionsAsync(bool showWhenInputIsEmpty)
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
        var args = new OptionsSearchEventArgs<TOption>()
        {
            Items = [],
            Text = _textInput ?? string.Empty,
        };

        await OnOptionsSearch.InvokeAsync(args);

        _internalFilteredItems = [.. args.Items?.Take(MaximumOptionsSearch) ?? []];
        _inProgress = false;
    }

    /// <summary />
    private Task DisplayFilteredOptionsAsync() => DisplayFilteredOptionsAsync(showWhenInputIsEmpty: true);

    /// <summary>
    /// When the user clicks the "x" button or presses Backspace with an empty input, remove the selected or latest item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private async Task RemoveSelectedItemAsync(TOption? item)
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
        _internalSelectedItems.Clear();

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(_internalSelectedItems);
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

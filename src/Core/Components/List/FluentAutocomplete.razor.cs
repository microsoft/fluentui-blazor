using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentAutocomplete<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    public static string AccessibilityItemIndexOfCount = "{0} ({1} of {2})";
    public static string AccessibilitySelected = "Selected {0}";
    public static string AccessibilityNotFound = "No items found";
    public static string AccessibilityReachedMaxItems = "The maximum number of selected items has been reached.";
    public static string AccessibilityRemoveItem = "Remove {0}";
    internal const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/List/FluentAutocomplete.razor.js";

    public new FluentTextField? Element { get; set; } = default!;
    private Virtualize<TOption>? VirtualizationContainer { get; set; }
    private readonly Debouncer _debouncer = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentAutocomplete{TOption}"/> class.
    /// </summary>
    public FluentAutocomplete()
    {
        Multiple = true;
        Width = "100%";
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary />
    private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Gets or sets the text field value.
    /// </summary>
    [Parameter]
    public string ValueText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback that is invoked when the text field value changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueTextChanged { get; set; }

    /// <summary>
    /// Gets or sets the value of the input. This should be used with two-way binding.
    /// For the FluentAutocomplete component, use the <see cref="ValueText"/> property instead.
    /// </summary>
    [Parameter]
    [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override string? Value
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
        get => ValueText;
        set => base.Value = ValueText;
    }

    /// <summary>
    /// For <see cref="FluentAutocomplete{TOption}"/>, this property must be True.
    /// Set the <see cref="MaximumSelectedOptions"/> property to 1 to select just one item.
    /// </summary>
    public override bool Multiple
    {
        get
        {
            return base.Multiple;
        }

        set
        {
            if (value == false)
            {
                throw new ArgumentException("For FluentAutocomplete, this property must be True. Set the MaximumSelectedOptions property to 1 to select just one item.");
            }

            base.Multiple = true;
        }
    }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// </summary>
    [Parameter]
    public FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Specifies whether a form or an input field should have autocomplete "on" or "off" or another value.
    /// An Id value must be set to use this property.
    /// </summary>
    [Parameter]
    public string? AutoComplete { get; set; }

    /// <summary>
    /// Filter the list of options (items), using the text encoded by the user.
    /// </summary>
    [Parameter]
    public EventCallback<OptionsSearchEventArgs<TOption>> OnOptionsSearch { get; set; }

    /// <summary>
    /// Gets or sets the style applied to all <see cref="FluentOption{TOption}"/> of the component.
    /// </summary>
    [Parameter]
    public string? OptionStyle { get; set; }

    /// <summary>
    /// Gets or sets the css class applied to all <see cref="FluentOption{TOption}"/> of the component.
    /// </summary>
    [Parameter]
    public string? OptionClass { get; set; }

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
    /// Gets or sets the template for the <see cref="ListComponentBase{TOption}.SelectedOptions"/> items.
    /// </summary>
    [Parameter]
    public RenderFragment<TOption>? SelectedOptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the header content, placed at the top of the popup panel.
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<TOption>>? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the footer content, placed at the bottom of the popup panel.
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<TOption>>? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets the title and Aria-Label for the Scroll to previous button.
    /// </summary>
    [Parameter]
    public string TitleScrollToPrevious { get; set; } = "Previous";

    /// <summary>
    /// Gets or sets the title and Aria-Label for the Scroll to next button.
    /// </summary>
    [Parameter]
    public string TitleScrollToNext { get; set; } = "Next";

    /// <summary>
    /// Gets or sets the icon used for the Clear button. By default: Dismiss icon.
    /// </summary>
    [Parameter]
    public Icon? IconDismiss { get; set; } = new CoreIcons.Regular.Size16.Dismiss();

    /// <summary>
    /// Gets or sets the icon used for the Search button. By default: Search icon.
    /// </summary>
    [Parameter]
    public Icon? IconSearch { get; set; } = new CoreIcons.Regular.Size16.Search();

    /// <summary>
    /// Gets or sets whether the dropdown is shown when there are no items.
    /// </summary>
    [Parameter]
    public bool ShowOverlayOnEmptyResults { get; set; } = true;

    /// <summary>
    /// If true, the options list will be rendered with virtualization. This is normally used in conjunction with
    /// scrolling and causes the option list to fetch and render only the data around the current scroll viewport.
    /// This can greatly improve the performance when scrolling through large data sets.
    ///
    /// If you use <see cref="Virtualize"/>, you should supply a value for <see cref="ItemSize"/> and must
    /// ensure that every row renders with the same constant height.
    ///
    /// Generally it's preferable not to use <see cref="Virtualize"/> if the amount of data being rendered is small.
    /// </summary>
    [Parameter]
    public bool Virtualize { get; set; }

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. It defines an expected height in pixels for
    /// each row, allowing the virtualization mechanism to fetch the correct number of items to match the display
    /// size and to ensure accurate scrolling.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 50;

    /// <summary>
    /// Gets or sets the maximum height of the field to adjust its height in relation to selected elements.
    /// </summary>
    [Parameter]
    public string? MaxAutoHeight { get; set; }

    /// <summary>
    /// Gets or sets whether the currently selected item from the drop-down (if it is open) is selected.
    /// Default is false.
    /// </summary>
    [Parameter]
    public bool SelectValueOnTab { get; set; } = false;

    /// <summary />
    private string? ListStyleValue => new StyleBuilder()
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("display", "none", when: (Items == null || !Items.Any()) && (HeaderContent != null || FooterContent != null))
        .Build();

    /// <summary />
    private string ComponentWidth
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Width))
            {
                if (Multiple)
                {
                    return $"width: 250px; min-width: 250px;";
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return $"width: {Width}; min-width: {Width};";
            }
        }
    }

    /// <summary />
    private string IdScroll => $"{Id}-scroll";

    /// <summary />
    private string IdPopup => $"{Id}-popup";

    /// <summary />
    private bool IsMultiSelectOpened { get; set; } = false;

    /// <summary />
    private bool IsReachedMaxItems { get; set; } = false;

    /// <summary />
    private TOption? SelectableItem { get; set; }

    /// <summary />
    protected override async Task InputHandlerAsync(ChangeEventArgs e)
    {
        if (ReadOnly || Disabled)
        {
            return;
        }

        ValueText = e.Value?.ToString() ?? string.Empty;
        await RaiseValueTextChangedAsync(ValueText);

        if (MaximumSelectedOptions > 0 && SelectedOptions?.Count() >= MaximumSelectedOptions)
        {
            IsReachedMaxItems = true;
            return;
        }

        IsReachedMaxItems = false;
        IsMultiSelectOpened = true;

        var args = new OptionsSearchEventArgs<TOption>()
        {
            Items = Items ?? Array.Empty<TOption>(),
            Text = ValueText,
        };

        if (ImmediateDelay > 0)
        {
            await _debouncer.DebounceAsync(ImmediateDelay, () => InvokeAsync(() => OnOptionsSearch.InvokeAsync(args)));
        }
        else
        {
            await OnOptionsSearch.InvokeAsync(args);
        }

        Items = args.Items?.Take(MaximumOptionsSearch);

        SelectableItem = Items != null
            ? Items.FirstOrDefault()
            : default;

        if (VirtualizationContainer != null)
        {
            await VirtualizationContainer.RefreshDataAsync();
        }
    }

    private ValueTask<ItemsProviderResult<TOption>> LoadFilteredItemsAsync(ItemsProviderRequest request)
    {
        if (Items is null)
        {
            return ValueTask.FromResult(
                new ItemsProviderResult<TOption>(
                    Array.Empty<TOption>(),
                    0));
        }

        return ValueTask.FromResult(
            new ItemsProviderResult<TOption>(
                Items.Skip(request.StartIndex).Take(request.Count),
                Items.Count()));
    }

    private static readonly KeyCode[] CatchOnly = new[] { KeyCode.Escape, KeyCode.Enter, KeyCode.Backspace, KeyCode.Down, KeyCode.Up };
    private static readonly KeyCode[] PreventOnly = CatchOnly.Except(new[] { KeyCode.Backspace }).ToArray();
    private static readonly KeyCode[] SelectValueOnTabOnly = new[] { KeyCode.Tab };

    /// <summary />
    protected async Task KeyDownHandlerAsync(FluentKeyCodeEventArgs e)
    {
        switch (e.Key)
        {
            case KeyCode.Escape:
                await KeyDown_EscapeAsync();
                break;

            case KeyCode.Enter:
            case KeyCode.Tab:
                if (IsMultiSelectOpened)
                {
                    var optionDisabled = SelectableItem != null && OptionDisabled != null
                                       ? OptionDisabled.Invoke(SelectableItem)
                                       : false;
                    if (optionDisabled)
                    {
                        await KeyDown_EscapeAsync();
                    }
                    else
                    {
                        await KeyDown_EnterAsync();
                    }
                }
                else
                {
                    await OnDropDownExpandedAsync();
                }
                break;

            case KeyCode.Backspace:
                await KeyDown_BackspaceAsync();
                break;

            case KeyCode.Down:
                if (IsMultiSelectOpened)
                {
                    await KeyDown_ArrowDownAsync();
                }
                else
                {
                    await OnDropDownExpandedAsync();
                }
                break;

            case KeyCode.Up:
                await KeyDown_ArrowUpAsync();
                break;
        }

        // Escape
        Task KeyDown_EscapeAsync()
        {
            IsMultiSelectOpened = false;
            return Task.CompletedTask;
        }

        // Backspace
        async Task KeyDown_BackspaceAsync()
        {
            // Remove last selected item
            if (string.IsNullOrEmpty(ValueText) &&
                SelectedOptions != null && SelectedOptions.Any())
            {
                await RemoveSelectedItemAsync(SelectedOptions.LastOrDefault());
                IsReachedMaxItems = false;
                return;
            }

            // Remove last char
            if (!string.IsNullOrEmpty(ValueText))
            {
                await InputHandlerAsync(new ChangeEventArgs()
                {
                    Value = ValueText[..^1],
                });
                return;
            }
        }

        // ArrowUp
        async Task KeyDown_ArrowUpAsync()
        {
            if (Items != null && Items.Any())
            {
                var index = Items.ToList().IndexOf(SelectableItem ?? Items.First());

                // Previous available item
                for (var i = index - 1; i >= 0; i--)
                {
                    var item = Items.ElementAt(i);
                    var disabled = OptionDisabled?.Invoke(item) ?? false;

                    if (!disabled)
                    {
                        SelectableItem = Items.ElementAt(i);
                        break;
                    }
                }

                if (Module != null)
                {
                    await Module.InvokeVoidAsync("scrollToFirstSelectable", IdPopup, false);
                }
            }
        }

        // ArrowDown
        async Task KeyDown_ArrowDownAsync()
        {
            if (Items != null && Items.Any())
            {
                var index = Items.ToList().IndexOf(SelectableItem ?? Items.First());

                // Next available item
                for (var i = index + 1; i < Items.Count(); i++)
                {
                    var item = Items.ElementAt(i);
                    var disabled = OptionDisabled?.Invoke(item) ?? false;

                    if (!disabled)
                    {
                        SelectableItem = Items.ElementAt(i);
                        break;
                    }
                }

                if (Module != null)
                {
                    await Module.InvokeVoidAsync("scrollToFirstSelectable", IdPopup, true);
                }
            }
        }

        // Enter
        async Task KeyDown_EnterAsync()
        {
            if (!IsMultiSelectOpened)
            {
                return;
            }

            if (Items != null && Items.Any() && SelectableItem != null)
            {
                await OnSelectedItemChangedHandlerAsync(SelectableItem);
            }

            SelectableItem = default;
            IsMultiSelectOpened = false;
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
        }
    }

    /// <summary />
    protected Task OnDropDownExpandedAsync()
    {
        return InputHandlerAsync(new ChangeEventArgs()
        {
            Value = ValueText,
        });
    }

    /// <summary />
    protected async Task OnClearAsync()
    {
        RemoveAllSelectedItems();
        ValueText = string.Empty;
        await RaiseValueTextChangedAsync(ValueText);
        await RaiseChangedEventsAsync();

        if (Module != null)
        {
            await Module.InvokeVoidAsync("focusOn", Id);
        }
    }

    /// <summary />
    protected override async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        ValueText = string.Empty;
        await RaiseValueTextChangedAsync(ValueText);

        IsMultiSelectOpened = false;
        await base.OnSelectedItemChangedHandlerAsync(item);
        await DisplayLastSelectedItemAsync();
    }

    /// <summary />
    public async Task RemoveSelectedItemAsync(TOption? item)
    {
        if (item == null)
        {
            return;
        }

        RemoveSelectedItem(item);
        await RaiseChangedEventsAsync();

        if (Module != null)
        {
            await Module.InvokeVoidAsync("focusOn", Id);
        }
    }

    /// <summary />
    private async Task DisplayLastSelectedItemAsync()
    {
        if (Module != null)
        {
            await Module.InvokeVoidAsync("displayLastSelectedItem", Id);
        }
    }

    /// <summary />
    private string? GetAutocompleteAriaLabel()
    {
        // No items found
        if (IsMultiSelectOpened && Items?.Any() == false)
        {
            return AccessibilityNotFound;
        }

        // Reached Max Items
        if (IsReachedMaxItems)
        {
            return AccessibilityReachedMaxItems;
        }

        // Selected {0}
        if (IsMultiSelectOpened && SelectableItem != null)
        {
            var item = GetOptionText(SelectableItem) ?? string.Empty;

            if (Items != null && SelectableItem != null)
            {
                var count = Items.Count();
                var current = Items.ToList().IndexOf(SelectableItem) + 1;
                return string.Format(AccessibilityItemIndexOfCount, item, current, count);
            }

            return item;
        }

        // Selected items
        if (SelectedOptions != null && SelectedOptions.Any())
        {
            return string.Format(AccessibilitySelected, string.Join(", ", SelectedOptions.Select(i => GetOptionText(i))));
        }

        // Default
        return GetAriaLabel() ?? Label ?? Placeholder;
    }

    /// <summary />
    private async Task RaiseValueTextChangedAsync(string value)
    {
        if (ValueTextChanged.HasDelegate)
        {
            await ValueTextChanged.InvokeAsync(ValueText);
        }

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(ValueText);
        }

    }
}

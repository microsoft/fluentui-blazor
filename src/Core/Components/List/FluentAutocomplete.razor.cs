using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentAutocomplete<TOption> : ListComponentBase<TOption>
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/List/FluentAutocomplete.razor.js";
    private string _valueText = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentAutocomplete{TOption}"/> class.
    /// </summary>
    public FluentAutocomplete()
    {
        Multiple = true;
        Width = "100%";
    }

    /// <summary />
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary />
    private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Sets the placeholder value of the element, generally used to provide a hint to the user.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

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
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

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
    /// Gets or sets the maximum number of options (items) selected. Exceeding this value, the user must delete some elements in order to select new ones.
    /// </summary>
    [Parameter]
    public int? MaximumSelectedOptions { get; set; }

    /// <summary>
    /// Template for the <see cref="ListComponentBase{TOption}.Items"/> items.
    /// </summary>
    [Parameter]
    public RenderFragment<TOption>? OptionTemplate { get; set; }

    /// <summary>
    /// Template for the <see cref="ListComponentBase{TOption}.SelectedOptions"/> items.
    /// </summary>
    [Parameter]
    public RenderFragment<TOption>? SelectedOptionTemplate { get; set; }

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
    private bool IsMultiSelectOpened { get; set; }

    /// <summary />
    private TOption? SelectableItem { get; set; }

    /// <summary />
    protected virtual async Task InputHandlerAsync(ChangeEventArgs e)
    {
        _valueText = e.Value?.ToString() ?? string.Empty;

        if (MaximumSelectedOptions > 0 && SelectedOptions?.Count() >= MaximumSelectedOptions)
        {
            return;
        }

        IsMultiSelectOpened = true;

        var args = new OptionsSearchEventArgs<TOption>()
        {
            Items = Items ?? Array.Empty<TOption>(),
            Text = _valueText,
        };

        await OnOptionsSearch.InvokeAsync(args);

        Items = args.Items.Take(MaximumOptionsSearch);
        SelectableItem = Items.FirstOrDefault();
    }

    /// <summary />
    protected virtual async Task KeyDownHandlerAsync(KeyboardEventArgs e)
    {
        switch (e.Code)
        {
            case "Escape":
                IsMultiSelectOpened = false;
                break;

            case "Enter":
            case "NumpadEnter":
                if (Items != null && Items.Any() && SelectableItem != null)
                {
                    await OnSelectedItemChangedHandlerAsync(SelectableItem);
                }

                SelectableItem = default;
                IsMultiSelectOpened = false;
                break;

            case "Backspace":
                if (string.IsNullOrEmpty(_valueText) &&
                    SelectedOptions != null && SelectedOptions.Any())
                {
                    await RemoveSelectedItemAsync(SelectedOptions.LastOrDefault());
                }

                break;

            case "ArrowDown":
                if (Items != null && Items.Any())
                {
                    var index = Items.ToList().IndexOf(SelectableItem ?? Items.First());
                    if (index < Items.Count() - 1)
                    {
                        SelectableItem = Items.ElementAt(index + 1);
                    }
                }
                break;

            case "ArrowUp":
                if (Items != null && Items.Any())
                {
                    var index = Items.ToList().IndexOf(SelectableItem ?? Items.First());
                    if (index > 0)
                    {
                        SelectableItem = Items.ElementAt(index - 1);
                    }
                }
                break;
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        }
    }

    /// <summary />
    protected virtual Task OnDropDownExpandedAsync()
    {
        return InputHandlerAsync(new ChangeEventArgs()
        {
            Value = _valueText,
        });
    }

    /// <summary />
    protected override async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        _valueText = string.Empty;
        IsMultiSelectOpened = false;
        await base.OnSelectedItemChangedHandlerAsync(item);
        await DisplayLastSelectedItemAsync();
    }

    /// <summary />
    protected virtual async Task RemoveSelectedItemAsync(TOption? item)
    {
        if (item == null)
        {
            return;
        }

        RemoveSelectedItem(item);
        await RaiseChangedEventsAsync();
    }

    /// <summary />
    private async Task DisplayLastSelectedItemAsync()
    {
        if (Module != null)
        {
            await Module.InvokeVoidAsync("displayLastSelectedItem", Id);
        }
    }

    private EventCallback<string> OnSelectCallback(TOption? item)
    {
        return EventCallback.Factory.Create<string>(this, (e) => OnSelectedItemChangedHandlerAsync(item));
    }
}

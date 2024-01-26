using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component that provides a list of options.
/// </summary>
/// <typeparam name="TOption"></typeparam>
public abstract partial class ListComponentBase<TOption> : FluentComponentBase, IAsyncDisposable where TOption : notnull
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/List/ListComponentBase.razor.js";

    private bool _multiple = false;
    private List<TOption> _selectedOptions = [];
    private TOption? _currentSelectedOption;
    protected readonly RenderFragment _renderOptions;

    private IJSObjectReference? Module { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    // We cascade the _internalListContext to descendants, which in turn call it to add themselves to the options list
    internal InternalListContext<TOption> _internalListContext;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        }
    }

    /// <summary />
    protected virtual string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected virtual string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    protected string? InternalValue
    {
        get
        {
            return GetOptionValue(SelectedOption);
        }
        set
        {
            if (value != null && OptionValue != null && Items != null)
            {
                TOption? item = Items.FirstOrDefault(i => GetOptionValue(i) == value);

                if (!Equals(item, SelectedOption))
                {
                    SelectedOption = item;

                    Value = value;
                    // Raise Changed events in another thread
                    RaiseChangedEventsAsync().ConfigureAwait(false);
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component or of the popup panel.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the text displayed just above the component.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content displayed just above the component.
    /// </summary>
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Gets or sets the text used on aria-label attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets if an indicator is showed that this input is required.
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the text used on aria-label attribute.
    /// </summary>
    [Parameter]
    [Obsolete("Use AriaLabel instead")]
    public virtual string? Title { get; set; }

    /// <summary>
    /// If true, will disable the list of items.
    /// </summary>
    [Parameter]
    public virtual bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// In this case list of FluentOptions
    /// </summary>
    [Parameter]
    public virtual RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display for each option.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionText { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which value to return for the selected item.
    /// Only for <see cref="FluentListbox{TOption}"/> and <see cref="FluentSelect{TOption}"/> components.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionValue { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an option is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionDisabled { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an option is initially selected.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionSelected { get; set; }

    /// <summary>
    /// Gets or sets the content source of all items to display in this list.
    /// Each item must be instantiated (cannot be null).
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? Items { get; set; }

    /// <summary>
    /// Gets or sets the selected item.
    /// ⚠️ Only available when Multiple = false.
    /// </summary>
    [Parameter]
    public virtual TOption? SelectedOption { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = false.
    /// </summary>
    [Parameter]
    public virtual EventCallback<TOption?> SelectedOptionChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected value.
    /// When Multiple = true this only reflects the first selected option value.
    /// </summary>
    [Parameter]
    public virtual string? Value { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = false.
    /// </summary>
    [Parameter]
    public virtual EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// If true, the user can select multiple elements.
    /// ⚠️ Only available for the FluentSelect and FluentListbox components.
    /// </summary>
    [Parameter]
    public virtual bool Multiple { get; set; }

    /// <summary>
    /// Gets or sets the template for the <see cref="ListComponentBase{TOption}.Items"/> items.
    /// </summary>
    [Parameter]
    public virtual RenderFragment<TOption>? OptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets all selected items.
    /// ⚠️ Only available when Multiple = true.
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? SelectedOptions { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = true.
    /// </summary>
    [Parameter]
    public virtual EventCallback<IEnumerable<TOption>?> SelectedOptionsChanged { get; set; }

    /// <summary />
    public ListComponentBase()
    {
        _internalListContext = new(this);

        Id = Identifier.NewId();

        OptionText = (item) => item?.ToString() ?? null;
        OptionValue = (item) => OptionText.Invoke(item) ?? item?.ToString() ?? null;

        _renderOptions = RenderOptions;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!Multiple)
        {
            bool isSetSelectedOption = false, isSetValue = false;
            TOption? newSelectedOption = default;
            string? newValue = null;

            foreach (var parameter in parameters)
            {
                switch (parameter.Name)
                {
                    case nameof(SelectedOption):
                        isSetSelectedOption = true;
                        newSelectedOption = (TOption?)parameter.Value;
                        break;
                    case nameof(Value):
                        isSetValue = true;
                        newValue = (string?)parameter.Value;
                        break;
                    default:
                        break;
                }
            }

            if (isSetSelectedOption && !Equals(_currentSelectedOption, newSelectedOption))
            {
                if (Items != null)
                {
                    if (Items.Contains(newSelectedOption))
                    {
                        _currentSelectedOption = newSelectedOption;
                    }
                    else
                    {
                        // If the selected option is not in the list of items, reset the selected option
                        _currentSelectedOption = SelectedOption = default;
                        await SelectedOptionChanged.InvokeAsync(SelectedOption);
                    }
                }
                else
                {
                    // If Items is null, we don't know if the selected option is in the list of items, so we just set it
                    _currentSelectedOption = newSelectedOption;
                }

                Value = GetOptionValue(_currentSelectedOption);
                await ValueChanged.InvokeAsync(Value);
            }
            else if (isSetValue && Items != null && GetOptionValue(_currentSelectedOption) != newValue)
            {
                newSelectedOption = Items.FirstOrDefault(item => GetOptionValue(item) == newValue);

                if (newSelectedOption != null)
                {
                    _currentSelectedOption = SelectedOption = newSelectedOption;
                }
                else
                {
                    // If the selected option is not in the list of items, reset the selected option
                    _currentSelectedOption = SelectedOption = default;
                    Value = null;
                    await ValueChanged.InvokeAsync(Value);
                }

                await SelectedOptionChanged.InvokeAsync(SelectedOption);

            }
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected override void OnInitialized()
    {
        if (_multiple != Multiple)
        {
            if (this is not FluentListbox<TOption> &&
                this is not FluentSelect<TOption> &&
                this is not FluentAutocomplete<TOption>)
            {
                throw new ArgumentException("Only FluentSelect, FluentListbox and FluentAutocomplete components support multi-selection mode. ", nameof(Multiple));
            }

            _multiple = Multiple;
        }

        if (!string.IsNullOrEmpty(Height) && string.IsNullOrEmpty(Id))
        {
            Id = Identifier.NewId();
        }
    }

    protected override void OnParametersSet()
    {

        if (this is not FluentListbox<TOption> || Items is null)
        {
            if (_internalListContext.ValueChanged.HasDelegate == false)
            {
                _internalListContext.ValueChanged = ValueChanged;
            }

            if (_internalListContext.SelectedOptionChanged.HasDelegate == false)
            {
                _internalListContext.SelectedOptionChanged = SelectedOptionChanged;
            }
        }

        if (InternalValue is null && Value is not null) // || InternalValue != Value)
        {
            InternalValue = Value;
        }

        if (Multiple)
        {
            if (SelectedOptions != null && _selectedOptions != SelectedOptions)
            {
                _selectedOptions = new List<TOption>(SelectedOptions);
            }

            if (SelectedOptions == null && Items != null && OptionSelected != null)
            {
                _selectedOptions.AddRange(Items.Where(item => OptionSelected.Invoke(item) && !_selectedOptions.Contains(item)));
                InternalValue = GetOptionValue(_selectedOptions.FirstOrDefault());
            }
        }
        else
        {
            if (SelectedOption == null && Items != null && OptionSelected != null)
            {
                TOption? item = Items.FirstOrDefault(i => OptionSelected.Invoke(i));
                InternalValue = GetOptionValue(item);
            }
        }

    }

    /// <summary />
    protected virtual bool DisabledItem(TOption item)
    {
        return Disabled;  // To allow overrides
    }

    /// <summary />
    protected virtual bool GetOptionSelected(TOption item)
    {
        if (Multiple)
        {

            if (_selectedOptions == null || item == null)
            {
                return false;
            }
            else if (OptionSelected != null && _selectedOptions.Contains(item))
            {
                return OptionSelected.Invoke(item);
            }

            else if (OptionValue != null && _selectedOptions != null)
            {
                foreach (var selectedItem in _selectedOptions)
                {
                    if (GetOptionValue(item) == GetOptionValue(selectedItem))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return _selectedOptions?.Contains(item) == true;
            }
        }
        else
        {
            if (OptionSelected != null)
            {
                return OptionSelected.Invoke(item);
            }
            else if (SelectedOption == null)
            {
                return false;
            }
            else if (OptionValue != null && SelectedOption != null)
            {
                return GetOptionValue(item) == GetOptionValue(SelectedOption);
            }
            else
            {
                return Equals(item, SelectedOption);
            }
        }
    }

    /// <summary />
    protected virtual string? GetOptionValue(TOption? item)
    {
        if (item != null)
        {
            return OptionValue.Invoke(item) ?? OptionText.Invoke(item) ?? item.ToString();
        }
        else
        {
            return null;
        }
    }

    protected virtual bool? GetOptionDisabled(TOption? item)
    {
        if (item != null)
        {
            if (OptionDisabled != null)
            {
                return OptionDisabled(item);
            }
            else
            {
                return Disabled;
            }
        }
        else
        {
            return null;
        }
    }

    /// <summary />
    protected virtual string? GetOptionText(TOption? item)
    {
        if (item != null)
        {
            return OptionText.Invoke(item) ?? item.ToString();
        }
        else
        {
            return null;
        }
    }

    /// <summary />
    protected virtual async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        if (Disabled || item == null)
        {
            return;
        }

        if (Multiple)
        {
            if (_selectedOptions.Contains(item))
            {
                RemoveSelectedItem(item);
                await RaiseChangedEventsAsync();
            }
            else
            {
                AddSelectedItem(item);
                await RaiseChangedEventsAsync();
            }
            if (!Equals(item, SelectedOption))
            {
                SelectedOption = item;
            }
        }
        else
        {
            if (!Equals(item, SelectedOption))
            {
                SelectedOption = item;
                await RaiseChangedEventsAsync();
            }
        }
    }

    /// <summary />
    protected virtual async Task RaiseChangedEventsAsync()
    {
        if (Multiple)
        {
            if (SelectedOptionsChanged.HasDelegate)
            {
                await SelectedOptionsChanged.InvokeAsync(_selectedOptions);
            }
        }
        else
        {
            if (SelectedOptionChanged.HasDelegate)
            {
                await SelectedOptionChanged.InvokeAsync(SelectedOption);
            }
        }
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(InternalValue);
        }
        StateHasChanged();
    }

    protected virtual async Task OnKeydownHandlerAsync(KeyboardEventArgs e)
    {
        if (e is null || Multiple)
        {
            return;
        }

        var id = await Module!.InvokeAsync<string>("getAriaActiveDescendant", Id);

        FluentOption<TOption> item = _internalListContext.Options.First(i => i.Id == id);

        await item.OnClickHandlerAsync();
    }

    /// <summary />
    protected virtual bool RemoveSelectedItem(TOption? item)
    {
        if (item == null)
        {
            return false;
        }

        return _selectedOptions.Remove(item);
    }

    /// <summary />
    protected virtual bool RemoveAllSelectedItems()
    {
        _selectedOptions = [];
        return true;
    }

    /// <summary />
    protected virtual void AddSelectedItem(TOption? item)
    {
        if (item == null)
        {
            return;
        }

        _selectedOptions.Add(item);
    }

    protected EventCallback<string> OnSelectCallback(TOption? item)
    {
        return EventCallback.Factory.Create<string>(this, (e) => OnSelectedItemChangedHandlerAsync(item));
    }

    /// <summary />
    protected internal string? GetAriaLabel()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return string.IsNullOrEmpty(AriaLabel) ? Title : AriaLabel;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Module is not null)
            {
                await Module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}

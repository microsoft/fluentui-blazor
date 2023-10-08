using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Component that provides a list of options.
/// </summary>
/// <typeparam name="TOption"></typeparam>

public abstract class ListComponentBase<TOption> : FluentComponentBase
{
    private bool _multiple = false;
    private List<TOption> _selectedOptions = new();

    // We cascade the InternalListContext to descendants, which in turn call it to add themselves to the options list
    internal InternalListContext<TOption> _internalListContext;

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
    /// Width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Height of the component or of the popup panel.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Text displayed just above the component
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Content displayed just above the component
    /// </summary>
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <summary>
    /// Text used on aria-label attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Text used on aria-label attribute.
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
    /// Function used to determine which text to display for each option.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionText { get; set; }

    /// <summary>
    /// Function used to determine which value to return for the selected item.
    /// Only for <see cref="FluentListbox{TOption}"/> and <see cref="FluentSelect{TOption}"/> components.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionValue { get; set; }

    /// <summary>
    /// Function used to determine if an option is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionDisabled { get; set; }

    /// <summary>
    /// Function used to determine if an option is initially selected.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionSelected { get; set; }

    /// <summary>
    /// Content source of all items to display in this list.
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
                _internalListContext.ValueChanged = ValueChanged;
            if (_internalListContext.SelectedOptionChanged.HasDelegate == false)
                _internalListContext.SelectedOptionChanged = SelectedOptionChanged;
        }

        if (InternalValue is null && Value is not null) // || InternalValue != Value)
        {
            InternalValue = Value;
        }



        base.OnParametersSet();
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
            return OptionValue.Invoke(item) ?? OptionText.Invoke(item) ?? item.ToString();
        else
            return null;
    }

    protected virtual bool? GetOptionDisabled(TOption? item)
    {
        if (item != null)
        {
            if (OptionDisabled != null)
                return OptionDisabled(item);
            else
                return Disabled;
        }
        else
            return null;
    }

    /// <summary />
    protected virtual string? GetOptionText(TOption? item)
    {
        if (item != null)
            return OptionText.Invoke(item) ?? item.ToString();
        else
            return null;
    }

    /// <summary />
    protected virtual async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        if (Disabled || item == null)
            return;

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

    /// <summary />
    protected virtual RenderFragment? GetListOptions(IEnumerable<TOption>? items)
    {
        if (items is not null)
        {
            return new RenderFragment(builder =>
            {
                foreach (TOption item in items)
                {
                    builder.OpenComponent<FluentOption<TOption>>(0);
                    builder.AddAttribute(1, "Value", GetOptionValue(item));
                    builder.AddAttribute(2, "Selected", GetOptionSelected(item));
                    builder.AddAttribute(3, "Disabled", GetOptionDisabled(item));

                    builder.AddAttribute(4, "ChildContent", (RenderFragment)(content =>
                    {
                        content.AddContent(5, GetOptionText(item));
                        if (item!.GetType().IsGenericType && item.GetType().GetGenericTypeDefinition() == typeof(Option<>))
                        {
                            Option<string>? t = item as Option<string>;
                            if (t is not null)
                            {
                                (Icon Value, Color? Color, string? Slot) = t.Icon;
                                if (Value != null)
                                {
                                    content.OpenComponent<FluentIcon<Icon>>(6);
                                    content.AddAttribute(7, "Value", Value);

                                    if (Slot is not null)
                                        content.AddAttribute(8, "Slot", Slot);
                                    if (Color is not null)
                                        content.AddAttribute(9, "Color", Color);

                                    content.CloseComponent();
                                }
                            }
                        }
                    }));

                    // Needed in fluent-listbox and fluent-select with mutliple select enabled
                    if (this is FluentListbox<TOption> ||
                       (this is FluentSelect<TOption> && Multiple) ||
                       (this is FluentAutocomplete<TOption> && Multiple))
                    {
                        builder.AddAttribute(10, "OnSelect", OnSelectCallback(item));
                    }

                    builder.CloseComponent();
                }
            });
        }
        else
        {
            return new RenderFragment(builder =>
            {
                builder.AddContent(0, ChildContent);
            });
        }

        EventCallback<string> OnSelectCallback(TOption? item)
        {
            return EventCallback.Factory.Create<string>(this, (e) => OnSelectedItemChangedHandlerAsync(item));
        }
    }

    /// <summary />
    protected virtual async Task OnChangedHandlerAsync(ChangeEventArgs e)
    {
        if (e.Value is not null && Items is not null && !Multiple)
        {
            TOption? item = Items.FirstOrDefault(i => GetOptionValue(i) == e.Value.ToString());

            await OnSelectedItemChangedHandlerAsync(item);
        }
        if (e.Value is not null && Multiple)
        {
            InternalValue = e.Value.ToString();
        }
    }

    /// <summary />
    protected virtual bool RemoveSelectedItem(TOption? item)
    {
        if (item == null)
            return false;

        return _selectedOptions.Remove(item);
    }

    /// <summary />
    protected virtual bool RemoveAllSelectedItems()
    {
        _selectedOptions = new();
        return true;
    }


    /// <summary />
    protected virtual void AddSelectedItem(TOption? item)
    {
        if (item == null)
            return;

        _selectedOptions.Add(item);
    }

    /// <summary />
    protected internal string? GetAriaLabel()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return string.IsNullOrEmpty(AriaLabel) ? Title : AriaLabel;
#pragma warning restore CS0618 // Type or member is obsolete
    }
}

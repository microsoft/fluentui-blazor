using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Component that provides a list of options.
/// </summary>
/// <typeparam name="TOption"></typeparam>

public abstract class ListBase<TOption> : FluentComponentBase
{
    private bool _multiple = false;
    private string? _value;
    private List<TOption> _selectedItems = new();

    // We cascade the InternalListContext to descendants, which in turn call it to add themselves to _options
    internal InternalListContext<TOption> _internalListContext;

    /// <summary />
    protected virtual string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected virtual string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Unique identifier.
    /// </summary>
    [Parameter]
    public virtual string? Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Text used on aria-label attribute.
    /// </summary>
    [Parameter]
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
    /// Function used to determine which text to return for the selected item.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionValue { get; set; }

    /// <summary>
    /// Function used to determine if an option is (initially) disabled.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionDisabled { get; set; }

    /// <summary>
    /// Function used to determine if an option is (initially) selected.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionSelected { get; set; }

    /// <summary>
    /// Data source of all items to display in this list.
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? Items { get; set; }

    /// <summary>
    /// Gets or sets the selected item.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual TOption? SelectedItem { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual EventCallback<TOption?> SelectedItemChanged { get; set; }

    /// <summary>
    /// Gets or sets the selected value (see <see cref="OptionValue"/>.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual string? Value
    {
        get
        {
            return GetOptionValue(SelectedItem);
        }
        set
        {
            if (value != null && OptionValue != null && Items != null)
            {
                var item = Items.FirstOrDefault(i => GetOptionValue(i) == value);

                if (!Equals(item, SelectedItem))
                {
                    SelectedItem = item;

                    // Raise Changed events in another thread
                    Task.Run(() => RaiseChangedEvents());
                }
            }
        }
    }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Available only if Multiple = false.
    /// </summary>
    [Parameter]
    public virtual EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// If true, the user can select multiple elements.
    /// ⚠️ Available only for the FluentSelect and FluentListbox component.
    /// </summary>
    [Parameter]
    public virtual bool Multiple { get; set; }


    /// <summary>
    /// Gets or sets all selected items.
    /// ⚠️ Available only if Multiple = true.
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? SelectedItems { get; set; }


    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Available only if Multiple = true.
    /// </summary>
    [Parameter]
    public virtual EventCallback<IEnumerable<TOption>?> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Function to define a customized multiselection text.
    /// ⚠️ Available only if Multiple = true.
    /// </summary>
    [Parameter]
    public virtual Func<IEnumerable<TOption>, string> MultiSelectionText { get; set; }



    /// <summary />
    public ListBase()
    {
        _internalListContext = new(this);


        OptionText = (item) => item?.ToString() ?? null;
        OptionValue = (item) => OptionText.Invoke(item) ?? item?.ToString() ?? null;
        MultiSelectionText = (items) =>
        {
            if (Items?.Any() == true)
            {
                return string.Join("; ", this.Items
                                             .Where(i => items.Contains(i))
                                             .Select(i => OptionText.Invoke(i) ?? i?.ToString() ?? "--"));
            }
            else
            {
                return string.Join("; ", items.Select(i => OptionText.Invoke(i) ?? i?.ToString() ?? "--"));
            }
        };
    }



    protected override void OnParametersSet()
    {

        if (!HasListControlWithGlobalChangedEvent)
        {
            if (_internalListContext.ValueChanged.HasDelegate == false)
            {
                _internalListContext.ValueChanged = ValueChanged;
            }
        }

        if (_multiple != Multiple)
        {
            if (this is not FluentListbox<TOption> && this is not FluentSelect<TOption>)
            {
                throw new ArgumentException("Only FluentSelect and FluentListbox components support multi-selection mode.");
            }

            _multiple = Multiple;
        }


        if (_selectedItems != SelectedItems)
        {
            _selectedItems = new List<TOption>(SelectedItems ?? Array.Empty<TOption>());

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
            if (SelectedItems == null || item == null)
            {
                return false;
            }
            else if (OptionValue != null && SelectedItems != null)
            {
                foreach (var selectedItem in SelectedItems)
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
                return SelectedItems?.Contains(item) == true;
            }
        }
        else
        {
            if (OptionSelected != null)
            {
                return OptionSelected.Invoke(item);
            }
            else if (SelectedItem == null)
            {
                return false;
            }
            else if (OptionValue != null && SelectedItem != null)
            {
                return GetOptionValue(item) == GetOptionValue(SelectedItem);
            }
            else
            {
                return Equals(item, SelectedItem);
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
                return OptionDisabled.Invoke(item);
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
    //protected virtual Task OnValueChangedHandlerAsync(TOption? item)
    //{
    //    return OnSelectedItemChangedHandlerAsync(item);
    //}

    /// <summary />
    protected virtual async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        if (Disabled || item == null)
            return;

        if (Multiple)
        {
            if (_selectedItems.Contains(item))
            {
                RemoveSelectedItem(item);
                await RaiseChangedEvents();
            }
            else
            {
                AddSelectedItem(item);
                await RaiseChangedEvents();
            }
        }
        else
        {
            if (!Equals(item, SelectedItem))
            {
                SelectedItem = item;
                await RaiseChangedEvents();
            }
        }
    }

    /// <summary />
    protected virtual async Task RaiseChangedEvents()
    {
        if (Multiple)
        {
            if (SelectedItemsChanged.HasDelegate)
                await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
        else
        {
            if (SelectedItemChanged.HasDelegate)
                await SelectedItemChanged.InvokeAsync(SelectedItem);

            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(Value);
        }
    }



    /// <summary />
    protected virtual RenderFragment? GetListOptions(IEnumerable<TOption>? items)
    {
        if (items is not null)
        {
            return new RenderFragment(builder =>
            {
                int i = 0;
                foreach (TOption item in items)
                {
                    builder.OpenComponent<FluentOption<TOption>>(i++);
                    builder.AddAttribute(i++, nameof(FluentOption<TOption>.Value), GetOptionValue(item));
                    builder.AddAttribute(i++, nameof(FluentOption<TOption>.Selected), GetOptionSelected(item));
                    builder.AddAttribute(i++, nameof(FluentOption<TOption>.Disabled), GetOptionDisabled(item));

                    builder.AddAttribute(i++, nameof(FluentOption<TOption>.ChildContent), (RenderFragment)(content =>
                    {
                        content.AddContent(i++, GetOptionText(item));
                    }));

                    // fluent-listbox doesn't support OnChange
                    if (HasListControlWithGlobalChangedEvent == false)
                    {
                        builder.AddAttribute(i++, nameof(FluentOption<TOption>.OnSelect), OnSelectCallback(item));
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
    protected virtual Task OnChangedHandlerAsync(ChangeEventArgs e)
    {
        if (e.Value is not null && Items is not null)
        {
            TOption? item = Items.FirstOrDefault(i => GetOptionText(i) == e.Value.ToString());

            return OnSelectedItemChangedHandlerAsync(item);
        }
        else
        {
            return Task.CompletedTask;
        }
    }

    /// <summary />
    protected virtual bool RemoveSelectedItem(TOption? item)
    {
        if (item == null)
            return false;

        return _selectedItems.Remove(item);
    }

    /// <summary />
    protected virtual void AddSelectedItem(TOption? item)
    {
        if (item == null)
            return;

        _selectedItems.Add(item);
    }

    // fluent-combobox and fluent-select can use OnChange event.
    // fluent-listbox cannot use OnChange event.
    private bool HasListControlWithGlobalChangedEvent
    {
        get
        {
            if (Multiple)
                return false;

            return (this is FluentCombobox<TOption>) || (this is FluentSelect<TOption>);
        }
    }
}

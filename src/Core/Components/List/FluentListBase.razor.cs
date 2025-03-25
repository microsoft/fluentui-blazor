// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public abstract partial class FluentListBase<TOption> : FluentInputBase<TOption>
{
    // List of items rendered with an ID to retrieve the element by ID.
    private Dictionary<string, TOption> InternalOptions { get; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the list.
    /// Default is `null`. Internally the component uses <see cref="ListAppearance.Outline"/> as default.
    /// </summary>
    [Parameter]
    public ListAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the size of the list.
    /// Default is `null`. Internally the component uses <see cref="ListSize.Medium"/> as default.
    /// </summary>
    [Parameter]
    public ListSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content source of all items to display in this list.
    /// Each item must be instantiated (cannot be null).
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? Items { get; set; }

    /// <summary>
    /// Gets or sets whether the list allows multiple selections.
    /// </summary>
    [Parameter]
    public bool Multiple { get; set; }

    /// <summary>
    /// Gets or sets the items that are selected in the list.
    /// </summary>
    [Parameter]
    public IEnumerable<TOption> SelectedItems { get; set; } = [];

    /// <summary>
    /// Event callback that is invoked when the selected items change.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TOption>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets the placeholder text to display when no item is selected.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the template for the <see cref="FluentListBase{TOption}.Items"/> items.
    /// </summary>
    [Parameter]
    public virtual RenderFragment<TOption>? OptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which value to apply to the option value attribute.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, string>? OptionValue { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display for each option.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, string>? OptionText { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an option is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, bool>? OptionDisabled { get; set; }

    /// <summary />
    internal string? AddOption(FluentOption option)
    {
        var id = option.Id ?? "";

        // Bound list
        if (Items != null)
        {
            if (option.Data is null)
            {
                InternalOptions.TryAdd(id, default!);
                return option.Id;
            }

            if (option.Data is TOption item)
            {
                InternalOptions.TryAdd(id, item);
                return option.Id;
            }
        }

        // Manual list using FluentOption
        if (typeof(TOption) == typeof(string) && option.Value is TOption value)
        {
            InternalOptions.TryAdd(id, value);
            return option.Id;
        }

        return null;
    }

    /// <summary />
    internal string? RemoveOption(FluentOption option)
    {
        var id = option.Id ?? "";

        if (InternalOptions.ContainsKey(id))
        {
            if (option.Data is TOption _ ||
                typeof(TOption) == typeof(string) && option.Value is TOption _)
            {
                InternalOptions.Remove(id);
                return option.Id;
            }
        }

        return null;
    }

    /// <summary />
    protected virtual bool GetOptionSelected(TOption? item)
    {
        if (item == null)
        {
            return false;
        }

        // Multiple items
        if (Multiple)
        {
            return SelectedItems?.Contains(item) ?? false;
        }

        // Single item
        return Equals(item, CurrentValue);
    }

    /// <summary />
    protected virtual string? GetOptionValue(TOption? item)
    {
        return OptionValue?.Invoke(item) ?? item?.ToString() ?? null;
    }

    /// <summary />
    protected virtual string? GetOptionText(TOption? item)
    {
        return OptionText?.Invoke(item) ?? item?.ToString() ?? null;
    }

    /// <summary />
    protected virtual bool GetOptionDisabled(TOption? item)
    {
        return OptionDisabled?.Invoke(item) ?? false;
    }

    /// <summary>
    /// Renders the list options.
    /// </summary>
    /// <returns></returns>
    protected virtual RenderFragment? RenderOptions() => InternalRenderOptions;

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }

    internal virtual async Task OnDropdownChangeHandlerAsync(DropdownEventArgs e)
    {
        // List of IDs received from the web component.
        var selectedIds = e.SelectedOptions?.Split(';', StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
        SelectedItems = selectedIds.Length > 0
                      ? InternalOptions.Where(kvp => selectedIds.Contains(kvp.Key, StringComparer.Ordinal)).Select(kvp => kvp.Value).ToList()
                      : Array.Empty<TOption>();

        if (SelectedItemsChanged.HasDelegate)
        {
            await SelectedItemsChanged.InvokeAsync(SelectedItems);
        }

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(SelectedItems.FirstOrDefault());
        }
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TOption result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary>
    /// For unit testing purposes only.
    /// </summary>
    internal bool InternalTryParseValueFromString(string? value, [MaybeNullWhen(false)] out TOption result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return TryParseValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary />
    internal InternalListContext<TOption> GetCurrentContext()
    {
        return new InternalListContext<TOption>(this);
    }
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
[CascadingTypeParameter(nameof(TOption))]
public abstract partial class FluentListBase<TOption> : FluentInputBase<TOption>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentListBase{TOption}"/> class.
    /// </summary>
    protected FluentListBase()
    {
        Id = Identifier.NewId();
    }

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
    /// Gets or sets the function used to determine if an option is initially selected.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, bool>? OptionSelected { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an option is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, bool>? OptionDisabled { get; set; }

    /// <summary />
    protected virtual bool GetOptionSelected(TOption? item)
    {
        return OptionSelected?.Invoke(item) ?? Equals(item, CurrentValue);
    }

    /// <summary />
    protected virtual string? GetOptionValue(TOption? item)
    {
        return OptionValue?.Invoke(item) ?? item?.ToString() ?? null;
    }

    /// <summary />
    protected virtual string? GetOptionText(TOption? item)
    {
        return OptionText?.Invoke(item) ?? item?.ToString() ?? string.Empty;
    }

    /// <summary />
    protected virtual bool GetOptionDisabled(TOption? item)
    {
        return OptionDisabled?.Invoke(item) ?? false;
    }

    /// <summary />
    protected virtual async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        if (Disabled ?? false || item == null)
        {
            return;
        }

        if (!Equals(item, CurrentValue))
        {
            // Assign the current value and raise the change event
            CurrentValue = item;
        }

        await Task.CompletedTask;
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

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TOption result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary />
    internal InternalListContext<TOption> GetCurrentContext() => new(this);
}

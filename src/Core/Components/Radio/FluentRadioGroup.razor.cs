// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Fluent Radio Group component.
/// Groups <see cref="FluentRadio{TValue}"/> components together.
/// </summary>
/// <typeparam name="TValue">The type of the value</typeparam>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentRadioGroup<TValue> : FluentInputBase<TValue>, IFluentComponentElementBase
{
    internal ConcurrentDictionary<string, FluentRadio<TValue>> InternalRadios { get; } = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentRadioGroup{TRadioValue}"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings used to initialize the radio group. This parameter cannot be null.</param>
    public FluentRadioGroup(LibraryConfiguration configuration) : base(configuration) { }

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the group. See <see cref="Orientation"/>.
    /// The default is <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="FluentRadio{TValue}"/> wrapping is enabled.
    /// This is applied when the <see cref="Orientation"/> is set to <see cref="Orientation.Horizontal"/>
    /// and there are no effect on the <see cref="Orientation.Vertical"/> orientation.
    /// </summary>
    [Parameter]
    public bool Wrap { get; set; }

    /// <summary>
    ///     
    /// </summary>
    [Parameter]
    public IEnumerable<TValue?>? Items { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which value to apply to the radio value attribute.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, string?>? RadioValue { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display for each radio checkedItem.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, string?>? RadioLabel { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an radio is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, bool>? RadioDisabled { get; set; }

    /// <summary>
    /// Gets or sets the child content to be rendering inside the <see cref="FluentRadioGroup{TRadioValue}"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "value");
        }
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary />
    internal async Task RadioChangeHandlerAsync(RadioEventArgs e)
    {
        if (InternalRadios.TryGetValue(e.Id ?? "", out var checkedItem))
        {
            var newValue = Items is null && checkedItem.Value is null
                         ? TryParseValueFromString(checkedItem.GetValue(), out var result, out var _) ? result : default
                         : checkedItem.Value;

            if (!object.ReferenceEquals(Value, newValue))
            {
                Value = newValue;

                if (ValueChanged.HasDelegate)
                {
                    await ValueChanged.InvokeAsync(newValue);
                }
            }
        }
    }

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
    internal string GetGroupName()
    {
        return string.IsNullOrEmpty(Name) ? $"{Id}-group" : Name;
    }

    /// <summary />
    internal string? AddRadio(FluentRadio<TValue> radio)
    {
        if (!string.IsNullOrEmpty(radio.Id) &&
            InternalRadios.TryAdd(radio.Id, radio))
        {
            return radio.Id;
        }

        return null;
    }

    /// <summary />
    internal string? RemoveRadio(FluentRadio<TValue> radio)
    {
        if (!string.IsNullOrEmpty(radio.Id) &&
            InternalRadios.TryRemove(radio.Id, out var _))
        {
            return radio.Id;
        }

        return null;
    }
}

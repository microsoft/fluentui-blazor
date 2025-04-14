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
public partial class FluentRadioGroup2<TValue> : FluentInputBase<TValue>, IFluentComponentElementBase
{
    private ConcurrentDictionary<string, FluentRadio2<TValue>> InternalRadios { get; } = new(StringComparer.Ordinal);

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
    /// Gets or sets the function used to determine which value to apply to the radio value attribute.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, string>? RadioValue { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display for each radio item.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, string>? RadioLabel { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an radio is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, bool>? RadioDisabled { get; set; }

    /// <summary>
    /// Gets or sets the child content to be rendering inside the <see cref="FluentRadioGroup{TValue}"/>.
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
    private Task RadioChangeHandlerAsync(RadioEventArgs e)
    {
        if (InternalRadios.Contains)
            Console.WriteLine($"RadioChangeHandlerAsync: {e.Value} {Id}");
        return Task.CompletedTask;
        //return ChangeHandlerAsync(new ChangeEventArgs() { Value = e.Value });
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

    internal string GetGroupName()
    {
        return string.IsNullOrEmpty(Name) ? $"{Id}-group" : Name;
    }

    internal Task<string?> AddRadioAsync(FluentRadio2<TValue> radio)
    {
        if (!string.IsNullOrEmpty(radio.Id) &&
            InternalRadios.TryAdd(radio.Id, radio))
        {
            return Task.FromResult<string?>(radio.Id);
        }

        return Task.FromResult<string?>(null);
    }

    internal Task RemoveRadioAsync(FluentRadio2<TValue> radio)
    {
        if (InternalRadios.Contains(radio))
        {
            InternalRadios.Remove(radio);
        }

        return Task.CompletedTask;
    }
}

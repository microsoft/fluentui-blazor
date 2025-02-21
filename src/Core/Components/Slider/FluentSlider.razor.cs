// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentSlider component, a slider control that allows users to select from a range of values.    
/// </summary>
/// <typeparam name="TValue"></typeparam>
public partial class FluentSlider<TValue> : FluentInputBase<TValue>
    where TValue : struct, IComparable<TValue>
{
    ///// <summary />
    //protected string? ClassValue => DefaultClassBuilder
    //    .AddClass("loading-button", when: () => LoadingOverlay)
    //    .Build();

    ///// <summary />
    //private string? SliderClass => DefaultClassBuilder
    //    .AddClass("readonly", when: ReadOnly)
    //    .Build();

    /// <summary>
    /// Gets or sets the size for the slider.
    /// </summary>
    [Parameter]
    public SliderSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the slider's minimal value.
    /// </summary>
    [Parameter]
    public TValue? Min { get; set; }

    /// <summary>
    /// Gets or sets the slider's maximum value.
    /// </summary>
    [Parameter]
    public TValue? Max { get; set; }

    /// <summary>
    /// Gets or sets the slider's step value.
    /// </summary>
    [Parameter]
    public TValue? Step { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the stacked components.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; }

    /// <summary>
    /// Gets or sets the selection mode.
    /// </summary>
    [Parameter]
    public SliderMode? Mode { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the value of the slider.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (typeof(TValue) != typeof(byte) &&
           typeof(TValue) != typeof(int) &&
           typeof(TValue) != typeof(long) &&
           typeof(TValue) != typeof(float) &&
           typeof(TValue) != typeof(double) &&
           typeof(TValue) != typeof(decimal))
        {
            throw new InvalidOperationException("FluentSlider only supports numeric types.");
        }
    }

    private int? GetTabIndexValue =>
        ReadOnly && (!AdditionalAttributes?.ContainsKey("tabindex") ?? true)
        ? -1
        : null;

    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
    /// </summary>
    /// <param name = "value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected override string? FormatValueAsString(TValue value)
    {
        return Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
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

    /// <summary>
    /// Parses a string to create the <see cref="Microsoft.AspNetCore.Components.Forms.InputBase{TValue}.Value"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">The result to inject into the Value.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }

    // Only for Unit Tests
    internal string? FormatValueAsStringOrNull(TValue? value)
    {
        if (value is null)
        {
            return null;
        }

        return FormatValueAsString(value.Value);
    }
}

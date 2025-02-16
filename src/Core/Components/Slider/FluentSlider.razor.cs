// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
//using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
///     
/// </summary>
/// <typeparam name="TValue"></typeparam>
public partial class FluentSlider<TValue> : FluentInputBase<TValue>
    where TValue : System.Numerics.INumber<TValue>
{
    //private TValue? _max;
    //private TValue? _min;
    //private bool updateSliderThumb;
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT
        + "Slider/FluentSlider.razor.js";

    /// <summary>
    /// Gets or sets the size for the slider.
    /// Default is <see cref="SliderSize.Medium"/>.
    /// </summary>
    [Parameter]
    public SliderSize? Size { get; set; } = SliderSize.Medium;

    /// <summary>
    /// Gets or sets the slider's minimal value.
    /// </summary>
    [Parameter]
    public TValue? Min { get; set; }

    /// <summary>
    /// Gets or sets the slider's maximum value.
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Max { get; set; }

    /// <summary>
    /// Gets or sets the slider's step value.
    /// </summary>
    [Parameter]
    public TValue? Step { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the stacked components.
    /// Default is <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    private IJSObjectReference? Module { get; set; }
    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            Module = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
        }
        else
        {
            //if (updateSliderThumb)
            //{
            //    updateSliderThumb = false;
            //    if (Module is not null)
            //    {
            //        Debounce.Run(100, async () =>
            //        {
            //            await Module!.InvokeVoidAsync("updateSlider", Element);
            //        });
            //    }
            //}
        }
    }

    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
    /// </summary>
    /// <param name = "value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected override string? FormatValueAsString(TValue? value)
    {
        // Avoiding a cast to IFormattable to avoid boxing.
        return value switch
        {
            null => null,
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }
}

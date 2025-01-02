// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.FluentUI.AspNetCore.Components.Utilities.InternalDebounce;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentSlider<TValue> : FluentInputBase<TValue>, IAsyncDisposable
    where TValue : System.Numerics.INumber<TValue>
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Slider/FluentSlider.razor.js";

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    private TValue? max;
    private TValue? min;
    private bool updateSliderThumb = false;
    private DebounceAction Debounce { get; init; }
    public FluentSlider()
    {
        Debounce = new DebounceAction();
    }

    /// <summary>
    /// Gets or sets the slider's minimal value.
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Min
    {
        get => min;
        set
        {
            if (min != value)
            {
                min = value;
                updateSliderThumb = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the slider's maximum value.
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Max
    {
        get => max;
        set
        {
            if (max != value)
            {
                max = value;
                updateSliderThumb = true;
            }
        }
    }

    public override TValue? Value
    {
        get => base.Value;
        set
        {
            if (base.Value != value)
            {
                base.Value = value;
                updateSliderThumb = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the slider's step value.
    /// </summary>
    [Parameter, EditorRequired]
    public TValue? Step { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the slider. See <see cref="AspNetCore.Components.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        }
        else
        {
            if (updateSliderThumb)
            {
                updateSliderThumb = false;
                if (Module is not null)
                {
                    Debounce.Run(100, async () =>
                    {
                        await Module!.InvokeVoidAsync("updateSlider", Element);
                    });
                }
            }
        }
    }

    protected override string? ClassValue
    {
        get
        {
            return new CssBuilder(base.ClassValue)
                .AddClass(Orientation.ToAttributeValue() ?? "horizontal")
                .Build();
        }
    }

    protected override void OnParametersSet()
    {
        ArgumentNullException.ThrowIfNull(Min, nameof(Min));
        ArgumentNullException.ThrowIfNull(Max, nameof(Max));
        ArgumentNullException.ThrowIfNull(Step, nameof(Step));
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, "The {0} field must be a number.", DisplayName ?? (FieldBound ? FieldIdentifier.FieldName : "(unknown)"));
            return false;
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

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Module is not null)
            {
                await Module.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}

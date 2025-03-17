// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentNumberField<TValue> : FluentInputBase<TValue>, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/TextField/FluentTextField.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// When true, spin buttons will not be rendered.
    /// </summary>
    [Parameter]
    public bool HideStep { get; set; }

    /// <summary>
    /// Allows associating a <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/datalist">datalist</see> to the element by <see href="https://developer.mozilla.org/en-US/docs/Web/API/Element/id">id</see>.
    /// </summary>
    [Parameter]
    public string? DataList { get; set; }

    /// <summary>
    /// Gets or sets the maximum length.
    /// </summary>
    [Parameter]
    public int MaxLength { get; set; } = 14;

    /// <summary>
    /// Gets or sets the minimum length.
    /// </summary>
    [Parameter]
    public int MinLength { get; set; } = 1;

    /// <summary>
    /// Gets or sets the size.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 20;

    /// <summary>
    /// Gets or sets the amount to increase/decrease the number with. Only use whole number when TValue is int or long.
    /// </summary>
    [Parameter]
    public string Step { get; set; } = _stepAttributeValue;

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    [Parameter]
    public string? Max { get; set; }

    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    [Parameter]
    public string? Min { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="AspNetCore.Components.Appearance" />.
    /// </summary>
    [Parameter]
    public FluentInputAppearance Appearance { get; set; } = FluentInputAppearance.Outline;

    /// <summary>
    /// Specifies whether a form or an input field should have autocomplete "on" or "off" or another value.
    /// An Id value must be set to use this property.
    /// </summary>
    [Parameter]
    public string? AutoComplete { get; set; }

    /// <summary>
    /// Gets or sets the error message to show when the field can not be parsed.
    /// </summary>
    [Parameter]
    public string ParsingErrorMessage { get; set; } = "The {0} field must be a (valid) number.";

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// If true, the min and max values will be automatically set based on the type of TValue,
    /// unless an explicit value for Min or Max is provided.
    /// </summary>
    [Parameter]
    public bool UseTypeConstraints { get; set; }

    private static readonly string _stepAttributeValue = GetStepAttributeValue();

    // If type constraints is true and min is null, set min to the minimum value of TValue.
    private string? MinValue => UseTypeConstraints && Min == null ? InputHelpers<TValue>.GetMinValue() : Min;

    // If type constraints is true and max is null, set max to the maximum value of TValue.
    private string? MaxValue => UseTypeConstraints && Max == null ? InputHelpers<TValue>.GetMaxValue() : Max;

    private static string GetStepAttributeValue()
    {
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (targetType == typeof(sbyte) ||
            targetType == typeof(byte) ||
            targetType == typeof(int) ||
            targetType == typeof(uint) ||
            targetType == typeof(long) ||
            targetType == typeof(ulong) ||
            targetType == typeof(short) ||
            targetType == typeof(ushort) ||
            targetType == typeof(float) ||
            targetType == typeof(double) ||
            targetType == typeof(decimal))
        {
            return "1";
        }
        else
        {
            throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
        }
    }

    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
    /// </summary>
    /// <param name = "value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected override string? FormatValueAsString(TValue? value)
    {
        // Directly convert to string using InvariantCulture for all types
        return value switch
        {
            null => null,
            sbyte @sbyte => BindConverter.FormatValue(Convert.ToInt16(@sbyte), CultureInfo.InvariantCulture),
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            uint @uint => BindConverter.FormatValue(@uint, CultureInfo.InvariantCulture)?.ToString(),
            ushort @ushort => BindConverter.FormatValue(@ushort, CultureInfo.InvariantCulture)?.ToString(),
            ulong @ulong => BindConverter.FormatValue(@ulong, CultureInfo.InvariantCulture)?.ToString(),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        try
        {
            if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
        }
        catch (ArgumentException)
        {
            result = default!;
        }

        validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, FieldBound ? FieldIdentifier.FieldName : UnknownBoundField);
        return false;
    }

    protected override void OnParametersSet()
    {
        InputHelpers<TValue>.ValidateInputParameters(Max, Min);
        base.OnParametersSet();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            await Module.InvokeVoidAsync("ensureCurrentValueMatch", Element);

            if (AutoComplete != null && !string.IsNullOrEmpty(Id))
            {
                await Module.InvokeVoidAsync("setControlAttribute", Id, "autocomplete", AutoComplete);
            }

        }
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

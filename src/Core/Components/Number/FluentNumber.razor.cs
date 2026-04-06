// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A numeric input component that allows users to enter and edit numeric values.
/// </summary>
public partial class FluentNumber<TValue> : FluentInputImmediateBase<string?>, IFluentComponentElementBase, ITooltipComponent
                                            where TValue : struct, INumber<TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentNumber{TValue}"/> class.
    /// </summary>
    public FluentNumber(LibraryConfiguration configuration) : base(configuration)
    {
        // Default conditions for the message
        MessageCondition = (field) =>
        {
            field.MessageIcon = FluentStatus.ErrorIcon;
            field.Message = Localizer[Localization.LanguageResource.TextInput_RequiredMessage];

            return FocusLost &&
                   (Required ?? false)
                   && !(Disabled ?? false)
                   && !ReadOnly
                   && string.IsNullOrEmpty(CurrentValueAsString);
        };

    }

    /// <inheritdoc />
    protected override string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .Build();

    /// <summary>
    /// Gets the CSS class to apply to the internal web-component.
    /// </summary>
    protected virtual string? ComponentStyleValue => new StyleBuilder()
        .AddStyle("width", Width)
        .Build();

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// </summary>
    [Parameter]
    public TextInputAppearance Appearance { get; set; } = TextInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the content to prefix the input component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? StartTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content to suffix the input component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? EndTemplate { get; set; }

    /// <summary>
    /// Gets or sets the width of the input field.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the size of the input. See <see cref="Components.TextInputSize"/>
    /// </summary>
    [Parameter]
    public TextInputSize? Size { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the culture of the component.
    /// By default <see cref="CultureInfo.CurrentCulture"/> to display using the OS culture.
    /// </summary>
    [Parameter]
    public virtual CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    [Parameter]
    public TValue? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    [Parameter]
    public TValue? Max { get; set; }

    /// <summary>
    /// Gets or sets the step increment. For decimal values, use e.g. Step="0.01".
    /// </summary>
    [Parameter]
    public TValue? Step { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "value");

            // Initialize the 'immediate' custom event for the immediate mode
            await InitializeImmediateAsync();

            // Set the mask pattern
            //if (!string.IsNullOrEmpty(MaskPattern))
            {
                // id: string, 
                // scale: number, 
                // radixChar: string, 
                // mapToRadix: string[], 
                // min: number, 
                // max: number, 
                // thousandsSeparator: string
                await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.TextMasked.applyNumberMask",
                    Id,
                    Culture.NumberFormat.NumberDecimalDigits,
                    Culture.NumberFormat.NumberDecimalSeparator,
                    new[] { Culture.NumberFormat.NumberDecimalSeparator, CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator },
                    Min.HasValue ? Min.Value : int.MinValue,
                    Max.HasValue ? Max.Value : int.MaxValue,
                    Culture.NumberFormat.NumberGroupSeparator);
            }
        }
    }

    /// <summary>
    /// Parses a string to create the <see cref="Microsoft.AspNetCore.Components.Forms.InputBase{TValue}.Value"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">The result to inject into the Value.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;
        return true;
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
}

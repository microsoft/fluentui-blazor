// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A text input component that allows the user to enter a HEX color code
/// or pick a color from a <see cref="FluentColorPicker"/> displayed in a popover.
/// </summary>
public partial class FluentColorPickerInput : FluentInputImmediateBase<string?>
{
    private static readonly Icon IconPalette = new CoreIcons.Regular.Size20.DrawingPalette();

    [GeneratedRegex(@"^#(?:[0-9a-fA-F]{3}|[0-9a-fA-F]{6})$", RegexOptions.None, matchTimeoutMilliseconds: 100)]
    private static partial Regex HexColorRegex();

    private FluentColorPicker _colorPicker = default!;

    private bool _isOpen;

    /// <summary />
    public FluentColorPickerInput(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();

        // Default message displayed when the value is invalid
        MessageCondition = (field) =>
        {
            if (!string.IsNullOrEmpty(CurrentValueAsString) && !HexColorRegex().IsMatch(CurrentValueAsString))
            {
                field.MessageIcon = FluentStatus.ErrorIcon;
                field.Message = Localizer[Localization.LanguageResource.ColorPickerInput_InvalidHexMessage];
                return true;
            }

            if (FocusLost && (Required ?? false) && !(Disabled ?? false) && !ReadOnly && string.IsNullOrEmpty(CurrentValueAsString))
            {
                field.MessageIcon = FluentStatus.ErrorIcon;
                field.Message = Localizer[Localization.LanguageResource.TextInput_RequiredMessage];
                return true;
            }

            return false;
        };
    }

    /// <summary />
    protected override string? ClassValue => DefaultClassBuilder
        .AddClass(base.CssClass)
        .AddClass("fluent-color-picker-input")
        .Build();

    /// <summary>
    /// Gets or sets the visual appearance of the text input.
    /// </summary>
    [Parameter]
    public TextInputAppearance Appearance { get; set; } = TextInputAppearance.Outline;

    /// <summary>
    /// Gets or sets the short hint displayed in the input before the user enters a value.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the width of the text input.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the size of the input.
    /// </summary>
    [Parameter]
    public TextInputSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the tooltip text shown when hovering over the input.
    /// </summary>
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the view used to display the color picker in the popover.
    /// Default is <see cref="ColorPickerView.SwatchPalette"/>.
    /// </summary>
    [Parameter]
    public ColorPickerView View { get; set; } = ColorPickerView.SwatchPalette;

    /// <summary>
    /// Gets or sets the orientation of the color items in the swatch palette view.
    /// Default is <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the custom color palette displayed in the Swatch or ColorWheel view.
    /// If not set, the default palette will be used.
    /// </summary>
    [Parameter]
    public IReadOnlyList<string>? Palette { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to find the closest color in the palette
    /// when the current value does not exactly match any palette color.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool FindClosestColor { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the input is enabled (interactive).
    /// When false, the input will not be interactive and will have a disabled appearance.
    /// </summary>
    private bool Enabled => !(Disabled ?? false) && !ReadOnly;

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = value;
        validationErrorMessage = null;

        if (string.IsNullOrEmpty(value))
        {
            return true;
        }

        if (!HexColorRegex().IsMatch(value))
        {
            validationErrorMessage = Localizer[Localization.LanguageResource.ColorPickerInput_InvalidHexMessage];
            return false;
        }

        return true;
    }

    private string? GetSwatchColor()
    {
        var value = CurrentValueAsString;

        if (!string.IsNullOrEmpty(value) && HexColorRegex().IsMatch(value))
        {
            return value;
        }

        return "transparent";
    }

    private async Task OnTextValueChangedAsync(string? value)
    {
        if (!string.Equals(CurrentValueAsString, value, StringComparison.Ordinal))
        {
            CurrentValueAsString = value;

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(CurrentValue);
            }
        }
    }

    private async Task OnColorPickerSelectedAsync(string color)
    {
        if (!string.Equals(CurrentValueAsString, color, StringComparison.OrdinalIgnoreCase))
        {
            CurrentValueAsString = color;

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(CurrentValue);
            }
        }

        // Close the popover after selecting a color (except for the HSV square where the user keeps interacting).
        if (View != ColorPickerView.HsvSquare)
        {
            _isOpen = false;
        }
    }

    private Task TogglePopupAsync()
    {
        if (!Enabled)
        {
            return Task.CompletedTask;
        }

        _isOpen = !_isOpen;

        if (_isOpen && View == ColorPickerView.HsvSquare)
        {
            return _colorPicker.InitializeHsvAsync(force: true);
        }

        return Task.CompletedTask;
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentRating : FluentInputBase<int>
{
    private int? _mouseOverValue;

    public FluentRating() => Id = Identifier.NewId();

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    [Parameter] public int MaxValue { get; set; } = 10;


    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="AspNetCore.Components.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter] public Color? IconColor { get; set; } 

    /// <summary>
    /// Selected or hovered icon.
    /// </summary>
    [Parameter] public Icon IconFull { get; set; } = new CoreIcons.Filled.Size20.Star();

    /// <summary>
    /// Non-selected item icon.
    /// </summary>
    [Parameter] public Icon IconEmpty { get; set; } = new CoreIcons.Regular.Size20.Star();

    private Icon GetIcon(int index) => index <= (_mouseOverValue ?? Value) ? FullIcon : EmptyIcon;

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out int result, [NotNullWhen(false)] out string?
        validationErrorMessage)
    {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture,
                                                   "The {0} field must be a number.",
                                                   FieldBound ? FieldIdentifier.FieldName : UnknownBoundField);
            return false;
        }
    }

    protected internal async Task HandleKeyDownAsync(KeyboardEventArgs keyboardEventArgs)
    {
        if (Disabled || ReadOnly) { return; }

        var value = Value + keyboardEventArgs.Key switch
        {
            "ArrowRight" when keyboardEventArgs.ShiftKey => MaxValue - Value,
            "ArrowRight" => 1,
            "ArrowLeft" when keyboardEventArgs.ShiftKey => -Value,
            "ArrowLeft" => -1,
            _ => 1,
        };

        _mouseOverValue = null;
        if (value > MaxValue)
        {
            value = MaxValue;
        }
        else if (value < 0)
        {
            value = 0;
        }

        await SetCurrentValueAsync(value);
    }

    private void OnPointerOut() => _mouseOverValue = null;
    private void OnPointerOver(int value) => _mouseOverValue = value;
    private async Task OnClickAsync(int value)
    {
        if (value == Value) { value = 0; }
        await SetCurrentValueAsync(value);
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentRating : FluentInputBase<int>
{
    private int? _mouseOverValue;
    private bool _mouseOverDisabled;

    public FluentRating() => Id = Identifier.NewId();

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    [Parameter]
    public int MaxValue { get; set; } = 5;

    /// <summary>
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="AspNetCore.Components.Color"/> enumeration. Defaults to Accent.
    /// </summary>
    [Parameter]
    public Color? IconColor { get; set; }

    /// <summary>
    /// Gets or sets the icon drawing and fill color to a custom value.
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb) or CSS variable.
    /// ⚠️ Only available when Color is set to Color.Custom.
    /// </summary>
    [Parameter]
    public string? IconCustomColor { get; set; }

    /// <summary>
    /// The icon width.
    /// </summary>
    [Parameter]
    public string IconWidth { get; set; } = "20px";

    /// <summary>
    /// The icon to display when the rating value is greater than or equal to the item's value.
    /// </summary>
    [Parameter]
    public Icon IconFilled { get; set; } = new CoreIcons.Filled.Size20.Star();

    /// <summary>
    /// The icon to display when the rating value is less than the item's value.
    /// </summary>
    [Parameter]
    public Icon IconOutline { get; set; } = new CoreIcons.Regular.Size20.Star();

    /// <summary>
    /// Fires when hovered value changes. Value will be null if no rating item is hovered.
    /// </summary>
    [Parameter]
    public EventCallback<int?> OnPointerOver { get; set; }

    private Icon GetIcon(int index) => index <= (_mouseOverValue ?? Value) ? IconFilled : IconOutline;

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

    protected internal async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        if (Disabled || ReadOnly)
        {
            return;
        }

        int value;
        switch (e.Key)
        {
            case "ArrowRight" when e.ShiftKey: value = MaxValue; break;
            case "ArrowRight" or "ArrowUp": value = Math.Min(Value + 1, MaxValue); break;
            case "ArrowLeft" when e.ShiftKey: value = 1; break;
            case "ArrowLeft" or "ArrowDown": value = Math.Max(Value - 1, 1); break;
            default: return;
        }

        _mouseOverValue = null;
        _mouseOverDisabled = true;

        await SetCurrentValueAsync(value);
    }

    private async Task OnPointerOutAsync()
    {
        _mouseOverValue = null;
        _mouseOverDisabled = false;
        if (OnPointerOver.HasDelegate)
        {
            await OnPointerOver.InvokeAsync(_mouseOverValue);
        }
    }

    private async Task OnPointerOverAsync(int value)
    {
        if (_mouseOverDisabled)
        {
            return;
        }

        _mouseOverValue = value;
        if (OnPointerOver.HasDelegate)
        {
            await OnPointerOver.InvokeAsync(_mouseOverValue);
        }
    }

    private async Task OnClickAsync(int value)
    {
        if (value == Value && AllowReset)
        {
            value = 0;
            _mouseOverValue = null;
            _mouseOverDisabled = true;
        }
        await SetCurrentValueAsync(value);
    }
}

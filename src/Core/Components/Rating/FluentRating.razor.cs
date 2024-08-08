using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentRating : FluentInputBase<int>
{
    private bool _updatingCurrentValue = false;
    private int? _hoverValue = null;

    /// <summary />
    protected override string? ClassValue => new CssBuilder(base.ClassValue)
        .AddClass("fluent-rating")
        .Build();

    /// <summary />
    protected override string? StyleValue => new StyleBuilder(base.StyleValue).Build();

    /// <summary>
    /// Gets or sets the number of elements.
    /// </summary>
    [Parameter]
    public int Max { get; set; } = 5;

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
    /// Gets or sets the icon drawing and fill color. 
    /// Value comes from the <see cref="Color"/> enumeration. Defaults to Accent.
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
    public string IconWidth { get; set; } = "28px";

    /// <summary>
    /// Gets or sets a value that whether to allow clear when click again.
    /// </summary>
    [Parameter]
    public bool AllowReset { get; set; } = false;

    /// <summary>
    /// Fires when hovered value changes. Value will be null if no rating item is hovered.
    /// </summary>
    [Parameter]
    public EventCallback<int?> OnHoverValueChanged { get; set; }

    /// <summary />
    private string GroupName => Id ?? $"rating-{Id}";

    /// <summary />
    private Icon GetIcon(int index) => index <= (_hoverValue ?? Value) ? IconFilled : IconOutline;

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out int result, [NotNullWhen(false)] out string? validationErrorMessage)
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

    /// <summary />
    private async Task OnClickAsync(int value, bool fromFocus = false)
    {
        _updatingCurrentValue = true;

        // Reset ?
        if (AllowReset && value == Value && !fromFocus)
        {
            await SetCurrentValueAsync(0);
            await UpdateHoverValueAsync(null);
        }
        else
        {
            await SetCurrentValueAsync(value);
        }
    }

    /// <summary />
    private async Task OnMouseEnterAsync(int value)
    {
        if (_updatingCurrentValue)
        {
            return;
        }

        await UpdateHoverValueAsync(value);
    }

    /// <summary />
    private async Task OnMouseLeaveAsync()
    {
        await UpdateHoverValueAsync(null);
    }

    /// <summary />
    private async Task UpdateHoverValueAsync(int? value)
    {
        if (_hoverValue == value)
        {
            return;
        }

        _hoverValue = value;

        if (OnHoverValueChanged.HasDelegate)
        {
            await OnHoverValueChanged.InvokeAsync(value);
        }
    }
}

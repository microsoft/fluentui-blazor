using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDesignSystemProvider : FluentComponentBase
{
    [Parameter]
    public bool? NoPaint { get; set; }

    [Parameter]
    public string? FillColor { get; set; }

    [Parameter]
    public string? AccentBaseColor { get; set; }

    [Parameter]
    public string? NeutralBaseColor { get; set; }

    [Parameter]
    public int? Density { get; set; }

    [Parameter]
    public int? DesignUnit { get; set; }

    [Parameter]
    public LocalizationDirection? Direction { get; set; } = LocalizationDirection.LeftToRight;
    [Parameter]
    public int? BaseHeightMultiplier { get; set; }

    [Parameter]
    public int? BaseHorizontalSpacingMultiplier { get; set; }

    [Parameter]
    public int? ControlCornerRadius { get; set; }
    [Parameter]
    public int? LayerCornerRadius { get; set; }

    [Parameter]
    public int? StrokeWidth { get; set; }

    [Parameter]
    public int? FocusStrokeWidth { get; set; }

    [Parameter]
    public float? DisabledOpacity { get; set; }

    [Parameter]
    public string? TypeRampMinus2FontSize { get; set; }

    [Parameter]
    public string? TypeRampMinus2LineHeight { get; set; }

    [Parameter]
    public string? TypeRampMinus1FontSize { get; set; }

    [Parameter]
    public string? TypeRampMinus1LineHeight { get; set; }

    [Parameter]
    public string? TypeRampBaseFontSize { get; set; }

    [Parameter]
    public string? TypeRampBaseLineHeight { get; set; }

    [Parameter]
    public string? TypeRampPlus1FontSize { get; set; }

    [Parameter]
    public string? TypeRampPlus1LineHeight { get; set; }

    [Parameter]
    public string? TypeRampPlus2FontSize { get; set; }

    [Parameter]
    public string? TypeRampPlus2LineHeight { get; set; }

    [Parameter]
    public string? TypeRampPlus3FontSize { get; set; }

    [Parameter]
    public string? TypeRampPlus3LineHeight { get; set; }

    [Parameter]
    public string? TypeRampPlus4FontSize { get; set; }

    [Parameter]
    public string? TypeRampPlus4LineHeight { get; set; }

    [Parameter]
    public string? TypeRampPlus5FontSize { get; set; }

    [Parameter]
    public string? TypeRampPlus5LineHeight { get; set; }

    [Parameter]
    public string? TypeRampPlus6FontSize { get; set; }

    [Parameter]
    public string? TypeRampPlus6LineHeight { get; set; }

    [Parameter]
    public int? AccentFillRestDelta { get; set; }

    [Parameter]
    public int? AccentFillHoverDelta { get; set; }

    [Parameter]
    public int? AccentFillActiveDelta { get; set; }

    [Parameter]
    public int? AccentFillFocusDelta { get; set; }

    [Parameter]
    public int? AccentForegroundRestDelta { get; set; }

    [Parameter]
    public int? AccentForegroundHoverDelta { get; set; }

    [Parameter]
    public int? AccentForegroundActiveDelta { get; set; }

    [Parameter]
    public int? AccentForegroundFocusDelta { get; set; }

    [Parameter]
    public int? NeutralFillRestDelta { get; set; }

    [Parameter]
    public int? NeutralFillHoverDelta { get; set; }

    [Parameter]
    public int? NeutralFillActiveDelta { get; set; }

    [Parameter]
    public int? NeutralFillFocusDelta { get; set; }

    [Parameter]
    public int? NeutralFillInputRestDelta { get; set; }

    [Parameter]
    public int? NeutralFillInputHoverDelta { get; set; }

    [Parameter]
    public int? NeutralFillInputActiveDelta { get; set; }

    [Parameter]
    public int? NeutralFillInputFocusDelta { get; set; }

    [Parameter]
    public int? NeutralFillLayerRestDelta { get; set; }

    [Parameter]
    public int? NeutralFillStealthRestDelta { get; set; }

    [Parameter]
    public int? NeutralFillStealthHoverDelta { get; set; }

    [Parameter]
    public int? NeutralFillStealthActiveDelta { get; set; }

    [Parameter]
    public int? NeutralFillStealthFocusDelta { get; set; }

    [Parameter]
    public int? NeutralFillStrongHoverDelta { get; set; }

    [Parameter]
    public int? NeutralFillStrongActiveDelta { get; set; }

    [Parameter]
    public int? NeutralFillStrongFocusDelta { get; set; }

    [Parameter]
    public int? NeutralStrokeDividerRestDelta { get; set; }

    [Parameter]
    public int? NeutralStrokeRestDelta { get; set; }

    [Parameter]
    public int? NeutralStrokeHoverDelta { get; set; }

    [Parameter]
    public int? NeutralStrokeActiveDelta { get; set; }

    [Parameter]
    public int? NeutralStrokeFocusDelta { get; set; }

    [Parameter]
    public float? BaseLayerLuminance { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

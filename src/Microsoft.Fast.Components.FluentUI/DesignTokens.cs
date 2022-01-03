using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

public class DesignTokens
{
    public DesignToken<string> BodyFont { get; set; }
    public DesignToken<int> BaseHeightMultiplier { get; set; }
    public DesignToken<int> BaseHorizontalSpacingMultiplier { get; set; }
    public DesignToken<float> BaseLayerLuminance { get; set; }
    public DesignToken<int> ControlCornerRadius { get; set; }
    public DesignToken<int> Density { get; set; }
    public DesignToken<int> DesignUnit { get; set; }
    public DesignToken<string> Direction { get; set; }
    public DesignToken<float> DisabledOpacity { get; set; }
    public DesignToken<int> StrokeWidth { get; set; }
    public DesignToken<int> FocusStrokeWidth { get; set; }
    public DesignToken<float> TypeRampBaseFontSize { get; set; }
    public DesignToken<float> TypeRampBaseLineHeight { get; set; }
    public DesignToken<float> TypeRampMinus1FontSize { get; set; }
    public DesignToken<float> TypeRampMinus1LineHeight { get; set; }
    public DesignToken<float> TypeRampMinus2FontSize { get; set; }
    public DesignToken<float> TypeRampMinus2LineHeight { get; set; }
    public DesignToken<float> TypeRampPlus1FontSize { get; set; }
    public DesignToken<float> TypeRampPlus1LineHeight { get; set; }
    public DesignToken<float> TypeRampPlus2FontSize { get; set; }
    public DesignToken<float> TypeRampPlus2LineHeight { get; set; }
    public DesignToken<float> TypeRampPlus3FontSize { get; set; }
    public DesignToken<float> TypeRampPlus3LineHeight { get; set; }
    public DesignToken<float> TypeRampPlus4FontSize { get; set; }
    public DesignToken<float> TypeRampPlus4LineHeight { get; set; }
    public DesignToken<float> TypeRampPlus5FontSize { get; set; }
    public DesignToken<float> TypeRampPlus5LineHeight { get; set; }
    public DesignToken<float> TypeRampPlus6FontSize { get; set; }
    public DesignToken<float> TypeRampPlus6LineHeight { get; set; }
    public DesignToken<int> AccentFillRestDelta { get; set; }
    public DesignToken<int> AccentFillHoverDelta { get; set; }
    public DesignToken<int> AccentFillActiveDelta { get; set; }
    public DesignToken<int> AccentFillFocusDelta { get; set; }
    public DesignToken<int> AccentForegroundRestDelta { get; set; }
    public DesignToken<int> AccentForegroundHoverDelta { get; set; }
    public DesignToken<int> AccentForegroundActiveDelta { get; set; }
    public DesignToken<int> AccentForegroundFocusDelta { get; set; }
    public DesignToken<int> NeutralFillRestDelta { get; set; }
    public DesignToken<int> NeutralFillHoverDelta { get; set; }
    public DesignToken<int> NeutralFillActiveDelta { get; set; }
    public DesignToken<int> NeutralFillFocusDelta { get; set; }
    public DesignToken<int> NeutralFillInputRestDelta { get; set; }
    public DesignToken<int> NeutralFillInputHoverDelta { get; set; }
    public DesignToken<int> NeutralFillInputActiveDelta { get; set; }
    public DesignToken<int> NeutralFillInputFocusDelta { get; set; }
    public DesignToken<int> NeutralFillStealthRestDelta { get; set; }
    public DesignToken<int> NeutralFillStealthHoverDelta { get; set; }
    public DesignToken<int> NeutralFillStealthActiveDelta { get; set; }
    public DesignToken<int> NeutralFillStealthFocusDelta { get; set; }
    public DesignToken<int> NeutralFillStrongRestDelta { get; set; }
    public DesignToken<int> NeutralFillStrongHoverDelta { get; set; }
    public DesignToken<int> NeutralFillStrongActiveDelta { get; set; }
    public DesignToken<int> NeutralFillStrongFocusDelta { get; set; }
    public DesignToken<int> NeutralFillLayerRestDelta { get; set; }
    public DesignToken<int> NeutralStrokeRestDelta { get; set; }
    public DesignToken<int> NeutralStrokeHoverDelta { get; set; }
    public DesignToken<int> NeutralStrokeActiveDelta { get; set; }
    public DesignToken<int> NeutralStrokeFocusDelta { get; set; }
    public DesignToken<int> NeutralStrokeDividerRestDelta { get; set; }

    public DesignTokens(IJSRuntime runtime)
    {
        BodyFont = new(runtime, DesignTokenNames.bodyFont);
        BaseHeightMultiplier = new(runtime, DesignTokenNames.baseHeightMultiplier);
        BaseHorizontalSpacingMultiplier = new(runtime, DesignTokenNames.baseHorizontalSpacingMultiplier);
        BaseLayerLuminance = new(runtime, DesignTokenNames.baseLayerLuminance);
        ControlCornerRadius = new(runtime, DesignTokenNames.controlCornerRadius);
        Density = new(runtime, DesignTokenNames.density);
        DesignUnit = new(runtime, DesignTokenNames.designUnit);
        Direction = new(runtime, DesignTokenNames.direction);
        DisabledOpacity = new(runtime, DesignTokenNames.disabledOpacity);
        StrokeWidth = new(runtime, DesignTokenNames.strokeWidth);
        FocusStrokeWidth = new(runtime, DesignTokenNames.focusStrokeWidth);
        TypeRampBaseFontSize = new(runtime, DesignTokenNames.typeRampBaseFontSize);
        TypeRampBaseLineHeight = new(runtime, DesignTokenNames.typeRampBaseLineHeight);
        TypeRampMinus1FontSize = new(runtime, DesignTokenNames.typeRampMinus1FontSize);
        TypeRampMinus1LineHeight = new(runtime, DesignTokenNames.typeRampMinus1LineHeight);
        TypeRampMinus2FontSize = new(runtime, DesignTokenNames.typeRampMinus2FontSize);
        TypeRampMinus2LineHeight = new(runtime, DesignTokenNames.typeRampMinus2LineHeight);
        TypeRampPlus1FontSize = new(runtime, DesignTokenNames.typeRampPlus1FontSize);
        TypeRampPlus1LineHeight = new(runtime, DesignTokenNames.typeRampPlus1LineHeight);
        TypeRampPlus2FontSize = new(runtime, DesignTokenNames.typeRampPlus2FontSize);
        TypeRampPlus2LineHeight = new(runtime, DesignTokenNames.typeRampPlus2LineHeight);
        TypeRampPlus3FontSize = new(runtime, DesignTokenNames.typeRampPlus3FontSize);
        TypeRampPlus3LineHeight = new(runtime, DesignTokenNames.typeRampPlus3LineHeight);
        TypeRampPlus4FontSize = new(runtime, DesignTokenNames.typeRampPlus4FontSize);
        TypeRampPlus4LineHeight = new(runtime, DesignTokenNames.typeRampPlus4LineHeight);
        TypeRampPlus5FontSize = new(runtime, DesignTokenNames.typeRampPlus5FontSize);
        TypeRampPlus5LineHeight = new(runtime, DesignTokenNames.typeRampPlus5LineHeight);
        TypeRampPlus6FontSize = new(runtime, DesignTokenNames.typeRampPlus6FontSize);
        TypeRampPlus6LineHeight = new(runtime, DesignTokenNames.typeRampPlus6LineHeight);
        AccentFillRestDelta = new(runtime, DesignTokenNames.accentFillRestDelta);
        AccentFillHoverDelta = new(runtime, DesignTokenNames.accentFillHoverDelta);
        AccentFillActiveDelta = new(runtime, DesignTokenNames.accentFillActiveDelta);
        AccentFillFocusDelta = new(runtime, DesignTokenNames.accentFillFocusDelta);
        AccentForegroundRestDelta = new(runtime, DesignTokenNames.accentForegroundRestDelta);
        AccentForegroundHoverDelta = new(runtime, DesignTokenNames.accentForegroundHoverDelta);
        AccentForegroundActiveDelta = new(runtime, DesignTokenNames.accentForegroundActiveDelta);
        AccentForegroundFocusDelta = new(runtime, DesignTokenNames.accentForegroundFocusDelta);
        NeutralFillRestDelta = new(runtime, DesignTokenNames.neutralFillRestDelta);
        NeutralFillHoverDelta = new(runtime, DesignTokenNames.neutralFillHoverDelta);
        NeutralFillActiveDelta = new(runtime, DesignTokenNames.neutralFillActiveDelta);
        NeutralFillFocusDelta = new(runtime, DesignTokenNames.neutralFillFocusDelta);
        NeutralFillInputRestDelta = new(runtime, DesignTokenNames.neutralFillInputRestDelta);
        NeutralFillInputHoverDelta = new(runtime, DesignTokenNames.neutralFillInputHoverDelta);
        NeutralFillInputActiveDelta = new(runtime, DesignTokenNames.neutralFillInputActiveDelta);
        NeutralFillInputFocusDelta = new(runtime, DesignTokenNames.neutralFillInputFocusDelta);
        NeutralFillStealthRestDelta = new(runtime, DesignTokenNames.neutralFillStealthRestDelta);
        NeutralFillStealthHoverDelta = new(runtime, DesignTokenNames.neutralFillStealthHoverDelta);
        NeutralFillStealthActiveDelta = new(runtime, DesignTokenNames.neutralFillStealthActiveDelta);
        NeutralFillStealthFocusDelta = new(runtime, DesignTokenNames.neutralFillStealthFocusDelta);
        NeutralFillStrongRestDelta = new(runtime, DesignTokenNames.neutralFillStrongRestDelta);
        NeutralFillStrongHoverDelta = new(runtime, DesignTokenNames.neutralFillStrongHoverDelta);
        NeutralFillStrongActiveDelta = new(runtime, DesignTokenNames.neutralFillStrongActiveDelta);
        NeutralFillStrongFocusDelta = new(runtime, DesignTokenNames.neutralFillStrongFocusDelta);
        NeutralFillLayerRestDelta = new(runtime, DesignTokenNames.neutralFillLayerRestDelta);
        NeutralStrokeRestDelta = new(runtime, DesignTokenNames.neutralStrokeRestDelta);
        NeutralStrokeHoverDelta = new(runtime, DesignTokenNames.neutralStrokeHoverDelta);
        NeutralStrokeActiveDelta = new(runtime, DesignTokenNames.neutralStrokeActiveDelta);
        NeutralStrokeFocusDelta = new(runtime, DesignTokenNames.neutralStrokeFocusDelta);
        NeutralStrokeDividerRestDelta = new(runtime, DesignTokenNames.neutralStrokeDividerRestDelta);
    }
}
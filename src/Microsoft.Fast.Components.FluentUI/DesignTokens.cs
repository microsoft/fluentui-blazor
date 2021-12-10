using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

public class DesignTokens
{
    public const string bodyFont = "bodyFont";
    public const string baseHeightMultiplier = "baseHeightMultiplier";
    public const string baseHorizontalSpacingMultiplier = "baseHorizontalSpacingMultiplier";
    public const string baseLayerLuminance = "baseLayerLuminance";
    public const string controlCornerRadius = "controlCornerRadius";
    public const string density = "density";
    public const string designUnit = "designUnit";
    public const string direction = "direction";
    public const string disabledOpacity = "disabledOpacity";
    public const string strokeWidth = "strokeWidth";
    public const string focusStrokeWidth = "focusStrokeWidth";
    public const string typeRampBaseFontSize = "typeRampBaseFontSize";
    public const string typeRampBaseLineHeight = "typeRampBaseLineHeight";
    public const string typeRampMinus1FontSize = "typeRampMinus1FontSize";
    public const string typeRampMinus1LineHeight = "typeRampMinus1LineHeight";
    public const string typeRampMinus2FontSize = "typeRampMinus2FontSize";
    public const string typeRampMinus2LineHeight = "typeRampMinus2LineHeight";
    public const string typeRampPlus1FontSize = "typeRampPlus1FontSize";
    public const string typeRampPlus1LineHeight = "typeRampPlus1LineHeight";
    public const string typeRampPlus2FontSize = "typeRampPlus2FontSize";
    public const string typeRampPlus2LineHeight = "typeRampPlus2LineHeight";
    public const string typeRampPlus3FontSize = "typeRampPlus3FontSize";
    public const string typeRampPlus3LineHeight = "typeRampPlus3LineHeight";
    public const string typeRampPlus4FontSize = "typeRampPlus4FontSize";
    public const string typeRampPlus4LineHeight = "typeRampPlus4LineHeight";
    public const string typeRampPlus5FontSize = "typeRampPlus5FontSize";
    public const string typeRampPlus5LineHeight = "typeRampPlus5LineHeight";
    public const string typeRampPlus6FontSize = "typeRampPlus6FontSize";
    public const string typeRampPlus6LineHeight = "typeRampPlus6LineHeight";
    public const string accentFillRestDelta = "accentFillRestDelta";
    public const string accentFillHoverDelta = "accentFillHoverDelta";
    public const string accentFillActiveDelta = "accentFillActiveDelta";
    public const string accentFillFocusDelta = "accentFillFocusDelta";
    public const string accentForegroundRestDelta = "accentForegroundRestDelta";
    public const string accentForegroundHoverDelta = "accentForegroundHoverDelta";
    public const string accentForegroundActiveDelta = "accentForegroundActiveDelta";
    public const string accentForegroundFocusDelta = "accentForegroundFocusDelta";
    public const string neutralFillRestDelta = "neutralFillRestDelta";
    public const string neutralFillHoverDelta = "neutralFillHoverDelta";
    public const string neutralFillActiveDelta = "neutralFillActiveDelta";
    public const string neutralFillFocusDelta = "neutralFillFocusDelta";
    public const string neutralFillInputRestDelta = "neutralFillInputRestDelta";
    public const string neutralFillInputHoverDelta = "neutralFillInputHoverDelta";
    public const string neutralFillInputActiveDelta = "neutralFillInputActiveDelta";
    public const string neutralFillInputFocusDelta = "neutralFillInputFocusDelta";
    public const string neutralFillStealthRestDelta = "neutralFillStealthRestDelta";
    public const string neutralFillStealthHoverDelta = "neutralFillStealthHoverDelta";
    public const string neutralFillStealthActiveDelta = "neutralFillStealthActiveDelta";
    public const string neutralFillStealthFocusDelta = "neutralFillStealthFocusDelta";
    public const string neutralFillStrongRestDelta = "neutralFillStrongRestDelta";
    public const string neutralFillStrongHoverDelta = "neutralFillStrongHoverDelta";
    public const string neutralFillStrongActiveDelta = "neutralFillStrongActiveDelta";
    public const string neutralFillStrongFocusDelta = "neutralFillStrongFocusDelta";
    public const string neutralFillLayerRestDelta = "neutralFillLayerRestDelta";
    public const string neutralStrokeRestDelta = "neutralStrokeRestDelta";
    public const string neutralStrokeHoverDelta = "neutralStrokeHoverDelta";
    public const string neutralStrokeActiveDelta = "neutralStrokeActiveDelta";
    public const string neutralStrokeFocusDelta = "neutralStrokeFocusDelta";
    public const string neutralStrokeDividerRestDelta = "neutralStrokeDividerRestDelta";

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
        BodyFont = new(runtime, bodyFont);
        BaseHeightMultiplier = new(runtime, baseHeightMultiplier);
        BaseHorizontalSpacingMultiplier = new(runtime, baseHorizontalSpacingMultiplier);
        BaseLayerLuminance = new(runtime, baseLayerLuminance);
        ControlCornerRadius = new(runtime, controlCornerRadius);
        Density = new(runtime, density);
        DesignUnit = new(runtime, designUnit);
        Direction = new(runtime, direction);
        DisabledOpacity = new(runtime, disabledOpacity);
        StrokeWidth = new(runtime, strokeWidth);
        FocusStrokeWidth = new(runtime, focusStrokeWidth);
        TypeRampBaseFontSize = new(runtime, typeRampBaseFontSize);
        TypeRampBaseLineHeight = new(runtime, typeRampBaseLineHeight);
        TypeRampMinus1FontSize = new(runtime, typeRampMinus1FontSize);
        TypeRampMinus1LineHeight = new(runtime, typeRampMinus1LineHeight);
        TypeRampMinus2FontSize = new(runtime, typeRampMinus2FontSize);
        TypeRampMinus2LineHeight = new(runtime, typeRampMinus2LineHeight);
        TypeRampPlus1FontSize = new(runtime, typeRampPlus1FontSize);
        TypeRampPlus1LineHeight = new(runtime, typeRampPlus1LineHeight);
        TypeRampPlus2FontSize = new(runtime, typeRampPlus2FontSize);
        TypeRampPlus2LineHeight = new(runtime, typeRampPlus2LineHeight);
        TypeRampPlus3FontSize = new(runtime, typeRampPlus3FontSize);
        TypeRampPlus3LineHeight = new(runtime, typeRampPlus3LineHeight);
        TypeRampPlus4FontSize = new(runtime, typeRampPlus4FontSize);
        TypeRampPlus4LineHeight = new(runtime, typeRampPlus4LineHeight);
        TypeRampPlus5FontSize = new(runtime, typeRampPlus5FontSize);
        TypeRampPlus5LineHeight = new(runtime, typeRampPlus5LineHeight);
        TypeRampPlus6FontSize = new(runtime, typeRampPlus6FontSize);
        TypeRampPlus6LineHeight = new(runtime, typeRampPlus6LineHeight);
        AccentFillRestDelta = new(runtime, accentFillRestDelta);
        AccentFillHoverDelta = new(runtime, accentFillHoverDelta);
        AccentFillActiveDelta = new(runtime, accentFillActiveDelta);
        AccentFillFocusDelta = new(runtime, accentFillFocusDelta);
        AccentForegroundRestDelta = new(runtime, accentForegroundRestDelta);
        AccentForegroundHoverDelta = new(runtime, accentForegroundHoverDelta);
        AccentForegroundActiveDelta = new(runtime, accentForegroundActiveDelta);
        AccentForegroundFocusDelta = new(runtime, accentForegroundFocusDelta);
        NeutralFillRestDelta = new(runtime, neutralFillRestDelta);
        NeutralFillHoverDelta = new(runtime, neutralFillHoverDelta);
        NeutralFillActiveDelta = new(runtime, neutralFillActiveDelta);
        NeutralFillFocusDelta = new(runtime, neutralFillFocusDelta);
        NeutralFillInputRestDelta = new(runtime, neutralFillInputRestDelta);
        NeutralFillInputHoverDelta = new(runtime, neutralFillInputHoverDelta);
        NeutralFillInputActiveDelta = new(runtime, neutralFillInputActiveDelta);
        NeutralFillInputFocusDelta = new(runtime, neutralFillInputFocusDelta);
        NeutralFillStealthRestDelta = new(runtime, neutralFillStealthRestDelta);
        NeutralFillStealthHoverDelta = new(runtime, neutralFillStealthHoverDelta);
        NeutralFillStealthActiveDelta = new(runtime, neutralFillStealthActiveDelta);
        NeutralFillStealthFocusDelta = new(runtime, neutralFillStealthFocusDelta);
        NeutralFillStrongRestDelta = new(runtime, neutralFillStrongRestDelta);
        NeutralFillStrongHoverDelta = new(runtime, neutralFillStrongHoverDelta);
        NeutralFillStrongActiveDelta = new(runtime, neutralFillStrongActiveDelta);
        NeutralFillStrongFocusDelta = new(runtime, neutralFillStrongFocusDelta);
        NeutralFillLayerRestDelta = new(runtime, neutralFillLayerRestDelta);
        NeutralStrokeRestDelta = new(runtime, neutralStrokeRestDelta);
        NeutralStrokeHoverDelta = new(runtime, neutralStrokeHoverDelta);
        NeutralStrokeActiveDelta = new(runtime, neutralStrokeActiveDelta);
        NeutralStrokeFocusDelta = new(runtime, neutralStrokeFocusDelta);
        NeutralStrokeDividerRestDelta = new(runtime, neutralStrokeDividerRestDelta);
    }
}
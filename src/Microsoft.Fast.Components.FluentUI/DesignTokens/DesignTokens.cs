using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public class DesignTokens
{
    public DesignToken<string> BodyFont { get; set; }
    public DesignToken<int?> BaseHeightMultiplier { get; set; }
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

    public DesignTokens(IJSRuntime jsRuntime)
    {
        BodyFont = new(jsRuntime, Constants.BodyFont);
        BaseHeightMultiplier = new(jsRuntime, Constants.BaseHeightMultiplier);
        BaseHorizontalSpacingMultiplier = new(jsRuntime, Constants.BaseHorizontalSpacingMultiplier);
        BaseLayerLuminance = new(jsRuntime, Constants.BaseLayerLuminance);
        ControlCornerRadius = new(jsRuntime, Constants.ControlCornerRadius);
        Density = new(jsRuntime, Constants.Density);
        DesignUnit = new(jsRuntime, Constants.DesignUnit);
        Direction = new(jsRuntime, Constants.Direction);
        DisabledOpacity = new(jsRuntime, Constants.DisabledOpacity);
        StrokeWidth = new(jsRuntime, Constants.StrokeWidth);
        FocusStrokeWidth = new(jsRuntime, Constants.FocusStrokeWidth);
        TypeRampBaseFontSize = new(jsRuntime, Constants.TypeRampBaseFontSize);
        TypeRampBaseLineHeight = new(jsRuntime, Constants.TypeRampBaseLineHeight);
        TypeRampMinus1FontSize = new(jsRuntime, Constants.TypeRampMinus1FontSize);
        TypeRampMinus1LineHeight = new(jsRuntime, Constants.TypeRampMinus1LineHeight);
        TypeRampMinus2FontSize = new(jsRuntime, Constants.TypeRampMinus2FontSize);
        TypeRampMinus2LineHeight = new(jsRuntime, Constants.TypeRampMinus2LineHeight);
        TypeRampPlus1FontSize = new(jsRuntime, Constants.TypeRampPlus1FontSize);
        TypeRampPlus1LineHeight = new(jsRuntime, Constants.TypeRampPlus1LineHeight);
        TypeRampPlus2FontSize = new(jsRuntime, Constants.TypeRampPlus2FontSize);
        TypeRampPlus2LineHeight = new(jsRuntime, Constants.TypeRampPlus2LineHeight);
        TypeRampPlus3FontSize = new(jsRuntime, Constants.TypeRampPlus3FontSize);
        TypeRampPlus3LineHeight = new(jsRuntime, Constants.TypeRampPlus3LineHeight);
        TypeRampPlus4FontSize = new(jsRuntime, Constants.TypeRampPlus4FontSize);
        TypeRampPlus4LineHeight = new(jsRuntime, Constants.TypeRampPlus4LineHeight);
        TypeRampPlus5FontSize = new(jsRuntime, Constants.TypeRampPlus5FontSize);
        TypeRampPlus5LineHeight = new(jsRuntime, Constants.TypeRampPlus5LineHeight);
        TypeRampPlus6FontSize = new(jsRuntime, Constants.TypeRampPlus6FontSize);
        TypeRampPlus6LineHeight = new(jsRuntime, Constants.TypeRampPlus6LineHeight);
        AccentFillRestDelta = new(jsRuntime, Constants.AccentFillRestDelta);
        AccentFillHoverDelta = new(jsRuntime, Constants.AccentFillHoverDelta);
        AccentFillActiveDelta = new(jsRuntime, Constants.AccentFillActiveDelta);
        AccentFillFocusDelta = new(jsRuntime, Constants.AccentFillFocusDelta);
        AccentForegroundRestDelta = new(jsRuntime, Constants.AccentForegroundRestDelta);
        AccentForegroundHoverDelta = new(jsRuntime, Constants.AccentForegroundHoverDelta);
        AccentForegroundActiveDelta = new(jsRuntime, Constants.AccentForegroundActiveDelta);
        AccentForegroundFocusDelta = new(jsRuntime, Constants.AccentForegroundFocusDelta);
        NeutralFillRestDelta = new(jsRuntime, Constants.NeutralFillRestDelta);
        NeutralFillHoverDelta = new(jsRuntime, Constants.NeutralFillHoverDelta);
        NeutralFillActiveDelta = new(jsRuntime, Constants.NeutralFillActiveDelta);
        NeutralFillFocusDelta = new(jsRuntime, Constants.NeutralFillFocusDelta);
        NeutralFillInputRestDelta = new(jsRuntime, Constants.NeutralFillInputRestDelta);
        NeutralFillInputHoverDelta = new(jsRuntime, Constants.NeutralFillInputHoverDelta);
        NeutralFillInputActiveDelta = new(jsRuntime, Constants.NeutralFillInputActiveDelta);
        NeutralFillInputFocusDelta = new(jsRuntime, Constants.NeutralFillInputFocusDelta);
        NeutralFillStealthRestDelta = new(jsRuntime, Constants.NeutralFillStealthRestDelta);
        NeutralFillStealthHoverDelta = new(jsRuntime, Constants.NeutralFillStealthHoverDelta);
        NeutralFillStealthActiveDelta = new(jsRuntime, Constants.NeutralFillStealthActiveDelta);
        NeutralFillStealthFocusDelta = new(jsRuntime, Constants.NeutralFillStealthFocusDelta);
        NeutralFillStrongRestDelta = new(jsRuntime, Constants.NeutralFillStrongRestDelta);
        NeutralFillStrongHoverDelta = new(jsRuntime, Constants.NeutralFillStrongHoverDelta);
        NeutralFillStrongActiveDelta = new(jsRuntime, Constants.NeutralFillStrongActiveDelta);
        NeutralFillStrongFocusDelta = new(jsRuntime, Constants.NeutralFillStrongFocusDelta);
        NeutralFillLayerRestDelta = new(jsRuntime, Constants.NeutralFillLayerRestDelta);
        NeutralStrokeRestDelta = new(jsRuntime, Constants.NeutralStrokeRestDelta);
        NeutralStrokeHoverDelta = new(jsRuntime, Constants.NeutralStrokeHoverDelta);
        NeutralStrokeActiveDelta = new(jsRuntime, Constants.NeutralStrokeActiveDelta);
        NeutralStrokeFocusDelta = new(jsRuntime, Constants.NeutralStrokeFocusDelta);
        NeutralStrokeDividerRestDelta = new(jsRuntime, Constants.NeutralStrokeDividerRestDelta);
    }
}
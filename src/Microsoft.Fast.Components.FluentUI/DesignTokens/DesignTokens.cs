using Microsoft.Extensions.Configuration;
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

    public DesignTokens(IJSRuntime jsRuntime, IConfiguration configuration)
    {
        BodyFont = new(jsRuntime, configuration, Constants.BodyFont);
        BaseHeightMultiplier = new(jsRuntime, configuration, Constants.BaseHeightMultiplier);
        BaseHorizontalSpacingMultiplier = new(jsRuntime, configuration, Constants.BaseHorizontalSpacingMultiplier);
        BaseLayerLuminance = new(jsRuntime, configuration, Constants.BaseLayerLuminance);
        ControlCornerRadius = new(jsRuntime, configuration, Constants.ControlCornerRadius);
        Density = new(jsRuntime, configuration, Constants.Density);
        DesignUnit = new(jsRuntime, configuration, Constants.DesignUnit);
        Direction = new(jsRuntime, configuration, Constants.Direction);
        DisabledOpacity = new(jsRuntime, configuration, Constants.DisabledOpacity);
        StrokeWidth = new(jsRuntime, configuration, Constants.StrokeWidth);
        FocusStrokeWidth = new(jsRuntime, configuration, Constants.FocusStrokeWidth);
        TypeRampBaseFontSize = new(jsRuntime, configuration, Constants.TypeRampBaseFontSize);
        TypeRampBaseLineHeight = new(jsRuntime, configuration, Constants.TypeRampBaseLineHeight);
        TypeRampMinus1FontSize = new(jsRuntime, configuration, Constants.TypeRampMinus1FontSize);
        TypeRampMinus1LineHeight = new(jsRuntime, configuration, Constants.TypeRampMinus1LineHeight);
        TypeRampMinus2FontSize = new(jsRuntime, configuration, Constants.TypeRampMinus2FontSize);
        TypeRampMinus2LineHeight = new(jsRuntime, configuration, Constants.TypeRampMinus2LineHeight);
        TypeRampPlus1FontSize = new(jsRuntime, configuration, Constants.TypeRampPlus1FontSize);
        TypeRampPlus1LineHeight = new(jsRuntime, configuration, Constants.TypeRampPlus1LineHeight);
        TypeRampPlus2FontSize = new(jsRuntime, configuration, Constants.TypeRampPlus2FontSize);
        TypeRampPlus2LineHeight = new(jsRuntime, configuration, Constants.TypeRampPlus2LineHeight);
        TypeRampPlus3FontSize = new(jsRuntime, configuration, Constants.TypeRampPlus3FontSize);
        TypeRampPlus3LineHeight = new(jsRuntime, configuration, Constants.TypeRampPlus3LineHeight);
        TypeRampPlus4FontSize = new(jsRuntime, configuration, Constants.TypeRampPlus4FontSize);
        TypeRampPlus4LineHeight = new(jsRuntime, configuration, Constants.TypeRampPlus4LineHeight);
        TypeRampPlus5FontSize = new(jsRuntime, configuration, Constants.TypeRampPlus5FontSize);
        TypeRampPlus5LineHeight = new(jsRuntime, configuration, Constants.TypeRampPlus5LineHeight);
        TypeRampPlus6FontSize = new(jsRuntime, configuration, Constants.TypeRampPlus6FontSize);
        TypeRampPlus6LineHeight = new(jsRuntime, configuration, Constants.TypeRampPlus6LineHeight);
        AccentFillRestDelta = new(jsRuntime, configuration, Constants.AccentFillRestDelta);
        AccentFillHoverDelta = new(jsRuntime, configuration, Constants.AccentFillHoverDelta);
        AccentFillActiveDelta = new(jsRuntime, configuration, Constants.AccentFillActiveDelta);
        AccentFillFocusDelta = new(jsRuntime, configuration, Constants.AccentFillFocusDelta);
        AccentForegroundRestDelta = new(jsRuntime, configuration, Constants.AccentForegroundRestDelta);
        AccentForegroundHoverDelta = new(jsRuntime, configuration, Constants.AccentForegroundHoverDelta);
        AccentForegroundActiveDelta = new(jsRuntime, configuration, Constants.AccentForegroundActiveDelta);
        AccentForegroundFocusDelta = new(jsRuntime, configuration, Constants.AccentForegroundFocusDelta);
        NeutralFillRestDelta = new(jsRuntime, configuration, Constants.NeutralFillRestDelta);
        NeutralFillHoverDelta = new(jsRuntime, configuration, Constants.NeutralFillHoverDelta);
        NeutralFillActiveDelta = new(jsRuntime, configuration, Constants.NeutralFillActiveDelta);
        NeutralFillFocusDelta = new(jsRuntime, configuration, Constants.NeutralFillFocusDelta);
        NeutralFillInputRestDelta = new(jsRuntime, configuration, Constants.NeutralFillInputRestDelta);
        NeutralFillInputHoverDelta = new(jsRuntime, configuration, Constants.NeutralFillInputHoverDelta);
        NeutralFillInputActiveDelta = new(jsRuntime, configuration, Constants.NeutralFillInputActiveDelta);
        NeutralFillInputFocusDelta = new(jsRuntime, configuration, Constants.NeutralFillInputFocusDelta);
        NeutralFillStealthRestDelta = new(jsRuntime, configuration, Constants.NeutralFillStealthRestDelta);
        NeutralFillStealthHoverDelta = new(jsRuntime, configuration, Constants.NeutralFillStealthHoverDelta);
        NeutralFillStealthActiveDelta = new(jsRuntime, configuration, Constants.NeutralFillStealthActiveDelta);
        NeutralFillStealthFocusDelta = new(jsRuntime, configuration, Constants.NeutralFillStealthFocusDelta);
        NeutralFillStrongRestDelta = new(jsRuntime, configuration, Constants.NeutralFillStrongRestDelta);
        NeutralFillStrongHoverDelta = new(jsRuntime, configuration, Constants.NeutralFillStrongHoverDelta);
        NeutralFillStrongActiveDelta = new(jsRuntime, configuration, Constants.NeutralFillStrongActiveDelta);
        NeutralFillStrongFocusDelta = new(jsRuntime, configuration, Constants.NeutralFillStrongFocusDelta);
        NeutralFillLayerRestDelta = new(jsRuntime, configuration, Constants.NeutralFillLayerRestDelta);
        NeutralStrokeRestDelta = new(jsRuntime, configuration, Constants.NeutralStrokeRestDelta);
        NeutralStrokeHoverDelta = new(jsRuntime, configuration, Constants.NeutralStrokeHoverDelta);
        NeutralStrokeActiveDelta = new(jsRuntime, configuration, Constants.NeutralStrokeActiveDelta);
        NeutralStrokeFocusDelta = new(jsRuntime, configuration, Constants.NeutralStrokeFocusDelta);
        NeutralStrokeDividerRestDelta = new(jsRuntime, configuration, Constants.NeutralStrokeDividerRestDelta);
    }
}
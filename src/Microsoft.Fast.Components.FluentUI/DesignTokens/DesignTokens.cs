namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public class DesignTokens
{
    public DesignToken<string> BodyFont { get; set; }
    public DesignToken<int?> BaseHeightMultiplier { get; set; }
    public DesignToken<int?> BaseHorizontalSpacingMultiplier { get; set; }
    public DesignToken<float?> BaseLayerLuminance { get; set; }
    public DesignToken<int?> ControlCornerRadius { get; set; }
    public DesignToken<int?> Density { get; set; }
    public DesignToken<int?> DesignUnit { get; set; }
    public DesignToken<string> Direction { get; set; }
    public DesignToken<float?> DisabledOpacity { get; set; }
    public DesignToken<int?> StrokeWidth { get; set; }
    public DesignToken<int?> FocusStrokeWidth { get; set; }
    public DesignToken<float?> TypeRampBaseFontSize { get; set; }
    public DesignToken<float?> TypeRampBaseLineHeight { get; set; }
    public DesignToken<float?> TypeRampMinus1FontSize { get; set; }
    public DesignToken<float?> TypeRampMinus1LineHeight { get; set; }
    public DesignToken<float?> TypeRampMinus2FontSize { get; set; }
    public DesignToken<float?> TypeRampMinus2LineHeight { get; set; }
    public DesignToken<float?> TypeRampPlus1FontSize { get; set; }
    public DesignToken<float?> TypeRampPlus1LineHeight { get; set; }
    public DesignToken<float?> TypeRampPlus2FontSize { get; set; }
    public DesignToken<float?> TypeRampPlus2LineHeight { get; set; }
    public DesignToken<float?> TypeRampPlus3FontSize { get; set; }
    public DesignToken<float?> TypeRampPlus3LineHeight { get; set; }
    public DesignToken<float?> TypeRampPlus4FontSize { get; set; }
    public DesignToken<float?> TypeRampPlus4LineHeight { get; set; }
    public DesignToken<float?> TypeRampPlus5FontSize { get; set; }
    public DesignToken<float?> TypeRampPlus5LineHeight { get; set; }
    public DesignToken<float?> TypeRampPlus6FontSize { get; set; }
    public DesignToken<float?> TypeRampPlus6LineHeight { get; set; }
    public DesignToken<int?> AccentFillRest { get; set; }
    public DesignToken<int?> AccentFillHover { get; set; }
    public DesignToken<int?> AccentFillActive { get; set; }
    public DesignToken<int?> AccentFillFocus { get; set; }
    public DesignToken<int?> AccentForegroundRest { get; set; }
    public DesignToken<int?> AccentForegroundHover { get; set; }
    public DesignToken<int?> AccentForegroundActive { get; set; }
    public DesignToken<int?> AccentForegroundFocus { get; set; }
    public DesignToken<int?> NeutralFillRest { get; set; }
    public DesignToken<int?> NeutralFillHover { get; set; }
    public DesignToken<int?> NeutralFillActive { get; set; }
    public DesignToken<int?> NeutralFillFocus { get; set; }
    public DesignToken<int?> NeutralFillInputRest { get; set; }
    public DesignToken<int?> NeutralFillInputHover { get; set; }
    public DesignToken<int?> NeutralFillInputActive { get; set; }
    public DesignToken<int?> NeutralFillInputFocus { get; set; }
    public DesignToken<int?> NeutralFillStealthRest { get; set; }
    public DesignToken<int?> NeutralFillStealthHover { get; set; }
    public DesignToken<int?> NeutralFillStealthActive { get; set; }
    public DesignToken<int?> NeutralFillStealthFocus { get; set; }
    public DesignToken<int?> NeutralFillStrongRest { get; set; }
    public DesignToken<int?> NeutralFillStrongHover { get; set; }
    public DesignToken<int?> NeutralFillStrongActive { get; set; }
    public DesignToken<int?> NeutralFillStrongFocus { get; set; }
    public DesignToken<int?> NeutralFillLayerRest { get; set; }
    public DesignToken<int?> NeutralStrokeRest { get; set; }
    public DesignToken<int?> NeutralStrokeHover { get; set; }
    public DesignToken<int?> NeutralStrokeActive { get; set; }
    public DesignToken<int?> NeutralStrokeFocus { get; set; }
    public DesignToken<int?> NeutralStrokeDividerRest { get; set; }

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
        AccentFillRest = new(jsRuntime, configuration, Constants.AccentFillRest);
        AccentFillHover = new(jsRuntime, configuration, Constants.AccentFillHover);
        AccentFillActive = new(jsRuntime, configuration, Constants.AccentFillActive);
        AccentFillFocus = new(jsRuntime, configuration, Constants.AccentFillFocus);
        AccentForegroundRest = new(jsRuntime, configuration, Constants.AccentForegroundRest);
        AccentForegroundHover = new(jsRuntime, configuration, Constants.AccentForegroundHover);
        AccentForegroundActive = new(jsRuntime, configuration, Constants.AccentForegroundActive);
        AccentForegroundFocus = new(jsRuntime, configuration, Constants.AccentForegroundFocus);
        NeutralFillRest = new(jsRuntime, configuration, Constants.NeutralFillRest);
        NeutralFillHover = new(jsRuntime, configuration, Constants.NeutralFillHover);
        NeutralFillActive = new(jsRuntime, configuration, Constants.NeutralFillActive);
        NeutralFillFocus = new(jsRuntime, configuration, Constants.NeutralFillFocus);
        NeutralFillInputRest = new(jsRuntime, configuration, Constants.NeutralFillInputRest);
        NeutralFillInputHover = new(jsRuntime, configuration, Constants.NeutralFillInputHover);
        NeutralFillInputActive = new(jsRuntime, configuration, Constants.NeutralFillInputActive);
        NeutralFillInputFocus = new(jsRuntime, configuration, Constants.NeutralFillInputFocus);
        NeutralFillStealthRest = new(jsRuntime, configuration, Constants.NeutralFillStealthRest);
        NeutralFillStealthHover = new(jsRuntime, configuration, Constants.NeutralFillStealthHover);
        NeutralFillStealthActive = new(jsRuntime, configuration, Constants.NeutralFillStealthActive);
        NeutralFillStealthFocus = new(jsRuntime, configuration, Constants.NeutralFillStealthFocus);
        NeutralFillStrongRest = new(jsRuntime, configuration, Constants.NeutralFillStrongRest);
        NeutralFillStrongHover = new(jsRuntime, configuration, Constants.NeutralFillStrongHover);
        NeutralFillStrongActive = new(jsRuntime, configuration, Constants.NeutralFillStrongActive);
        NeutralFillStrongFocus = new(jsRuntime, configuration, Constants.NeutralFillStrongFocus);
        NeutralFillLayerRest = new(jsRuntime, configuration, Constants.NeutralFillLayerRest);
        NeutralStrokeRest = new(jsRuntime, configuration, Constants.NeutralStrokeRest);
        NeutralStrokeHover = new(jsRuntime, configuration, Constants.NeutralStrokeHover);
        NeutralStrokeActive = new(jsRuntime, configuration, Constants.NeutralStrokeActive);
        NeutralStrokeFocus = new(jsRuntime, configuration, Constants.NeutralStrokeFocus);
        NeutralStrokeDividerRest = new(jsRuntime, configuration, Constants.NeutralStrokeDividerRest);
    }
}
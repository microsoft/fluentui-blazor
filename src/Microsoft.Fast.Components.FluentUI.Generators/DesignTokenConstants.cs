namespace Microsoft.Fast.Components.FluentUI.Generators;

/// <summary>
/// This class contains all the design tokens as const <c ref="string" />s where the value represents 
/// the <c ref="Type"> of the value of the design token
/// </summary>
public class DesignTokenConstants
{
    // General tokens
    public const string direction = "FluentUI.Direction";
    public const string disabledOpacity = "float?";

    // Density tokens
    public const string baseHeightMultiplier = "float?";
    public const string baseHorizontalSpacingMultiplier = "float?";
    public const string density = "int?";
    public const string designUnit = "int?";

    // Appearance tokens
    public const string controlCornerRadius = "int?";
    public const string layerCornerRadius = "int?";

    public const string strokeWidth = "float?";
    public const string focusStrokeWidth = "float?";

    // Typography values
    public const string bodyFont = "string";
    public const string typeRampBaseFontSize = "string";
    public const string typeRampBaseLineHeight = "string";
    public const string typeRampMinus1FontSize = "string";
    public const string typeRampMinus1LineHeight = "string";
    public const string typeRampMinus2FontSize = "string";
    public const string typeRampMinus2LineHeight = "string";
    public const string typeRampPlus1FontSize = "string";
    public const string typeRampPlus1LineHeight = "string";
    public const string typeRampPlus2FontSize = "string";
    public const string typeRampPlus2LineHeight = "string";
    public const string typeRampPlus3FontSize = "string";
    public const string typeRampPlus3LineHeight = "string";
    public const string typeRampPlus4FontSize = "string";
    public const string typeRampPlus4LineHeight = "string";
    public const string typeRampPlus5FontSize = "string";
    public const string typeRampPlus5LineHeight = "string";
    public const string typeRampPlus6FontSize = "string";
    public const string typeRampPlus6LineHeight = "string";

    // Color recipe values
    public const string baseLayerLuminance = "float?";

    public const string accentFillRestDelta = "int?";
    public const string accentFillHoverDelta = "int?";
    public const string accentFillActiveDelta = "int?";
    public const string accentFillFocusDelta = "int?";

    public const string accentForegroundRestDelta = "int?";
    public const string accentForegroundHoverDelta = "int?";
    public const string accentForegroundActiveDelta = "int?";
    public const string accentForegroundFocusDelta = "int?";

    public const string neutralFillRestDelta = "int?";
    public const string neutralFillHoverDelta = "int?";
    public const string neutralFillActiveDelta = "int?";
    public const string neutralFillFocusDelta = "int?";

    public const string neutralFillInputRestDelta = "int?";
    public const string neutralFillInputHoverDelta = "int?";
    public const string neutralFillInputActiveDelta = "int?";
    public const string neutralFillInputFocusDelta = "int?";

    public const string neutralFillInputAltRestDelta = "int?";
    public const string neutralFillInputAltHoverDelta = "int?";
    public const string neutralFillInputAltActiveDelta = "int?";
    public const string neutralFillInputAltFocusDelta = "int?";

    public const string neutralFillLayerRestDelta = "int?";
    public const string neutralFillLayerHoverDelta = "int?";
    public const string neutralFillLayerActiveDelta = "int?";

    public const string neutralFillLayerAltRestDelta = "int?";

    public const string neutralFillSecondaryRestDelta = "int?";
    public const string neutralFillSecondaryHoverDelta = "int?";
    public const string neutralFillSecondaryActiveDelta = "int?";
    public const string neutralFillSecondaryFocusDelta = "int?";

    public const string neutralFillStealthRestDelta = "int?";
    public const string neutralFillStealthHoverDelta = "int?";
    public const string neutralFillStealthActiveDelta = "int?";
    public const string neutralFillStealthFocusDelta = "int?";

    public const string neutralFillStrongRestDelta = "int?";
    public const string neutralFillStrongHoverDelta = "int?";
    public const string neutralFillStrongActiveDelta = "int?";
    public const string neutralFillStrongFocusDelta = "int?";

    public const string neutralStrokeRestDelta = "int?";
    public const string neutralStrokeHoverDelta = "int?";
    public const string neutralStrokeActiveDelta = "int?";
    public const string neutralStrokeFocusDelta = "int?";

    public const string neutralStrokeControlRestDelta = "int?";
    public const string neutralStrokeControlHoverDelta = "int?";
    public const string neutralStrokeControlActiveDelta = "int?";
    public const string neutralStrokeControlFocusDelta = "int?";

    public const string neutralStrokeDividerRestDelta = "int?";

    public const string neutralStrokeLayerRestDelta = "int?";
    public const string neutralStrokeLayerHoverDelta = "int?";
    public const string neutralStrokeLayerActiveDelta = "int?";

    public const string neutralStrokeStrongHoverDelta = "int?";
    public const string neutralStrokeStrongActiveDelta = "int?";
    public const string neutralStrokeStrongFocusDelta = "int?";

    // Color recipes
    public const string neutralBaseColor = "System.Drawing.Color";
    public const string accentBaseColor = "System.Drawing.Color";

    // Neutral Layer Card Container
    public const string neutralLayerCardContainer = "System.Drawing.Color";

    // Neutral Layer Floating
    public const string neutralLayerFloating = "System.Drawing.Color";

    // Neutral Layer 1
    public const string neutralLayer1 = "System.Drawing.Color";

    // Neutral Layer 2
    public const string neutralLayer2 = "System.Drawing.Color";

    // Neutral Layer 3
    public const string neutralLayer3 = "System.Drawing.Color";

    // Neutral Layer 4
    public const string neutralLayer4 = "System.Drawing.Color";

    public const string fillColor = "System.Drawing.Color";

    // Accent Fill
    public const string accentFillRest = "System.Drawing.Color";
    public const string accentFillHover = "System.Drawing.Color";
    public const string accentFillActive = "System.Drawing.Color";
    public const string accentFillFocus = "System.Drawing.Color";

    // Foreground On Accent
    public const string foregroundOnAccentRest = "System.Drawing.Color";
    public const string foregroundOnAccentHover = "System.Drawing.Color";
    public const string foregroundOnAccentActive = "System.Drawing.Color";
    public const string foregroundOnAccentFocus = "System.Drawing.Color";

    // Accent Foreground
    public const string accentForegroundRest = "System.Drawing.Color";
    public const string accentForegroundHover = "System.Drawing.Color";
    public const string accentForegroundActive = "System.Drawing.Color";
    public const string accentForegroundFocus = "System.Drawing.Color";

    // Accent Stroke Control
    public const string accentStrokeControlRest = "System.Drawing.Color";
    public const string accentStrokeControlHover = "System.Drawing.Color";
    public const string accentStrokeControlActive = "System.Drawing.Color";
    public const string accentStrokeControlFocus = "System.Drawing.Color";

    // Neutral Fill
    public const string neutralFillRest = "System.Drawing.Color";
    public const string neutralFillHover = "System.Drawing.Color";
    public const string neutralFillActive = "System.Drawing.Color";
    public const string neutralFillFocus = "System.Drawing.Color";

    // Neutral Fill Input
    public const string neutralFillInputRest = "System.Drawing.Color";
    public const string neutralFillInputHover = "System.Drawing.Color";
    public const string neutralFillInputActive = "System.Drawing.Color";
    public const string neutralFillInputFocus = "System.Drawing.Color";

    // Neutral Fill Input Alt
    public const string neutralFillInputAltRest = "System.Drawing.Color";
    public const string neutralFillInputAltHover = "System.Drawing.Color";
    public const string neutralFillInputAltActive = "System.Drawing.Color";
    public const string neutralFillInputAltFocus = "System.Drawing.Color";

    // Neutral Fill Layer
    public const string neutralFillLayerRest = "System.Drawing.Color";
    public const string neutralFillLayerHover = "System.Drawing.Color";
    public const string neutralFillLayerActive = "System.Drawing.Color";

    // Neutral Fill Layer Alt
    public const string neutralFillLayerAltRest = "System.Drawing.Color";

    // Neutral Fill Secondary
    public const string neutralFillSecondaryRest = "System.Drawing.Color";
    public const string neutralFillSecondaryHover = "System.Drawing.Color";
    public const string neutralFillSecondaryActive = "System.Drawing.Color";
    public const string neutralFillSecondaryFocus = "System.Drawing.Color";

    // Neutral Fill Stealth
    public const string neutralFillStealthRest = "System.Drawing.Color";
    public const string neutralFillStealthHover = "System.Drawing.Color";
    public const string neutralFillStealthActive = "System.Drawing.Color";
    public const string neutralFillStealthFocus = "System.Drawing.Color";

    // Neutral Fill Strong
    public const string neutralFillStrongRest = "System.Drawing.Color";

    public const string neutralFillStrongHover = "System.Drawing.Color";

    public const string neutralFillStrongActive = "System.Drawing.Color";

    public const string neutralFillStrongFocus = "System.Drawing.Color";


    // Neutral Foreground
    public const string neutralForegroundRest = "System.Drawing.Color";

    public const string neutralForegroundHover = "System.Drawing.Color";

    public const string neutralForegroundActive = "System.Drawing.Color";

    public const string neutralForegroundFocus = "System.Drawing.Color";


    // Neutral Foreground Hint
    public const string neutralForegroundHint = "System.Drawing.Color";


    // Neutral Stroke
    public const string neutralStrokeRest = "System.Drawing.Color";
    public const string neutralStrokeHover = "System.Drawing.Color";
    public const string neutralStrokeActive = "System.Drawing.Color";
    public const string neutralStrokeFocus = "System.Drawing.Color";

    // Neutral Stroke Control
    public const string neutralStrokeControlRest = "System.Drawing.Color";
    public const string neutralStrokeControlHover = "System.Drawing.Color";
    public const string neutralStrokeControlActive = "System.Drawing.Color";
    public const string neutralStrokeControlFocus = "System.Drawing.Color";


    // Neutral Stroke Divider
    public const string neutralStrokeDividerRest = "System.Drawing.Color";

    // Neutral Stroke Input
    public const string neutralStrokeInputRest = "System.Drawing.Color";
    public const string neutralStrokeInputHover = "System.Drawing.Color";
    public const string neutralStrokeInputActive = "System.Drawing.Color";
    public const string neutralStrokeInputFocus = "System.Drawing.Color";

    // Neutral Stroke Layer
    public const string neutralStrokeLayerRest = "System.Drawing.Color";
    public const string neutralStrokeLayerHover = "System.Drawing.Color";
    public const string neutralStrokeLayerActive = "System.Drawing.Color";

    // Neutral Stroke Strong
    public const string neutralStrokeStrongRest = "System.Drawing.Color";
    public const string neutralStrokeStrongHover = "System.Drawing.Color";
    public const string neutralStrokeStrongActive = "System.Drawing.Color";
    public const string neutralStrokeStrongFocus = "System.Drawing.Color";

    // Focus Stroke Outer
    public const string focusStrokeOuter = "System.Drawing.Color";

    // Focus Stroke Inner
    public const string focusStrokeInner = "System.Drawing.Color";
}

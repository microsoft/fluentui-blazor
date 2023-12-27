namespace Microsoft.FluentUI.AspNetCore.Components;

/*
 * See https://learn.microsoft.com/en-us/fluent-ui/web-components/getting-started/styling
 * And https://color.fast.design/
 */

public class ThemeToken
{
    /// <summary>
    /// Common used properties.
    /// </summary>
    public CommonProps Common { get; set; } = new();

    /// <summary>
    /// Color properties
    /// </summary>
    public ColorsProps Colors { get; set; } = new();

    /// <summary>
    /// Less common properties.
    /// </summary>
    public OthersProps Others { get; set; } = new();

    public class CommonProps
    {
        /// <summary>
        /// Gets or sets a value that may be applied to an element's styles and used as context for child color recipes.
        /// </summary>
        [ThemeTokenType(TokenType.Color)]
        public string? FillColor { get; set; }

        /// <summary>
        /// Set to a custom swatch to use for color recipes for layers and other neutral components.
        /// </summary>
        [ThemeTokenType(TokenType.Color)] 
        public string? NeutralBaseColor { get; set; }

        /// <summary>
        /// Used to specify the font string to apply to components. Note that this does not import fonts, so they must either be web standard, assumed to be installed, or imported at the top of your app.
        /// </summary>
        [ThemeTokenType(TokenType.Text)]
        public string? BodyFont { get; set; }

        /// <summary>
        /// Used to specify the font weight to apply to component
        /// </summary>
        [ThemeTokenType(TokenType.Number)]
        public double? FontWeight { get; set; }

        /// <summary>
        /// Sets the corner radius used by controls with backplates.
        /// Ex: Increase to 6px for slightly rounder buttons and text fields.
        /// </summary>
        [ThemeTokenType(TokenType.Number)] 
        public double? ControlCornerRadius { get; set; }

        /// <summary>
        /// Sets the corner radius used layers like cards, flyouts, and dialogs.
        /// Ex: Increase to 20px for very round cards.
        /// </summary>
        [ThemeTokenType(TokenType.Number)] 
        public double? LayerCornerRadius { get; set; }

        /// <summary>
        /// A modifier used with sizing tokens baseHeightMultiplier and baseHorizontalSpacingMultiplier.
        /// Ex: Set to 1 to increase control size or -1 to decrease.
        /// </summary>
        [ThemeTokenType(TokenType.Number)] 
        public double? Density { get; set; }
    }

    public class ColorsProps
    {
        /// <summary>
        /// Relies on textColor and contrastTarget to find the closest colors from the supplied palette that can be used for component states. For instance, colors needed to support white text and a 14px font (which requires 4.5:1 contrast).
        /// </summary>
        public RecipeStateDelta AccentFill { get; set; } = new();

        /// <summary>
        /// Commonly for link text or icon. Also for smaller elements that might not show up well using accentFill, for instance if your accent color is dark purple and you support a dark mode interface.
        /// Like accentFill this relies on textColor and contrastTarget to find the closest colors from the supplied palette that can be used for component states.
        /// </summary>
        public RecipeStateDelta AccentForeground { get; set; } = new();

        public RecipeState AccentStrokeControl { get; set; } = new();

        /// <summary>
        /// Technically this doesn't use the accent palette, but it's designed to be used over the accent palette. This algorithm simply returns black or white based on the provided contrastTarget. It returns white if possible, as a common treatment for an accent button is white text over the accent color.
        /// </summary>
        public RecipeState ForegroundOnAccent { get; set; } = new();

        /// <summary>
        /// The most basic fill used for buttons or other components.
        /// </summary>
        public RecipeStateDelta NeutralFill { get; set; } = new();

        /// <summary>
        /// Another basic fill, applied to input elements to allow easy differentiation from other components like buttons.
        /// </summary>
        public RecipeStateDelta NeutralFillInput { get; set; } = new();

        public RecipeStateDelta NeutralFillInputAlt { get; set; } = new();

        /// <summary>
        /// The only layer recipe that's relative to the container color instead of absolute. The most common example of this is a Card, which will be one layer color lighter than its container.
        /// </summary>
        public RecipeStateDelta NeutralFillLayer { get; set; } = new();

        public RecipeStateDelta NeutralFillSecondary { get; set; } = new();

        /// <summary>
        /// More subtle than neutralFill in that the resting state is transparent. Often used for low-priority features to draw less attention.
        /// </summary>
        public RecipeStateDelta NeutralFillStealth { get; set; } = new();

        public RecipeStateDelta NeutralFillStrong { get; set; } = new();

        /// <summary>
        /// Most common recipe, used for plain text or icons.
        /// </summary>
        public RecipeState NeutralForeground { get; set; } = new();

        /// <summary>
        /// Used for strong outline, either alone or with a fill.
        /// </summary>
        public RecipeStateDelta NeutralStroke { get; set; } = new();

        public RecipeStateDelta NeutralStrokeControl { get; set; } = new();

        public RecipeState neutralStrokeInput { get; set; } = new();

        public RecipeStateDelta NeutralStrokeLayer { get; set; } = new();

        public RecipeStateDelta NeutralStrokeStrong { get; set; } = new();
    }

    public class OthersProps
    {
        /// <summary>
        /// Type ramps
        /// </summary>
        public TypeRamps TypeRamps { get; set; } = new();

        /// <summary>
        ///  This value, multiplied by designUnit, sets the base height of most controls. Works with adaptive density values.
        /// </summary>
        public double? BaseHeightMultiplier { get; set; }

        /// <summary>
        /// This value, multiplied by designUnit, sets the internal horizontal padding of most controls. Works with adaptive density values.
        /// </summary>
        public double? BaseHorizontalSpacingMultiplier { get; set; }

        /// <summary>
        /// The unit size of the design grid. Used to calculate height and spacing sizes for controls.
        /// </summary>
        public double? DesignUnit { get; set; }

        /// <summary>
        /// Controls the width of the stroke of a component that has a stroke.
        /// </summary>
        public double? StrokeWidth { get; set; }

        /// <summary>
        /// Controls with width of the stroke of a component that has a stroke when it has document focus.
        /// </summary>
        public FocusStrokeProps FocusStroke { get; set; } = new();

        /// <summary>
        /// The opacity of disabled controls. Careful with values that are too high as the control may no longer look disabled. There are no contrast requirements for a disabled control.
        /// </summary>
        public double? DisabledOpacity { get; set; }

        public RecipeStateDelta ElevationShadowCard { get; set; } = new();

        public ElevationShadowProps ElevationShadow { get; set; } = new();

        /// <summary>
        /// Absolute layer colors derived from and starting at baseLayerLuminance.
        /// Layer one is lightest and the values darken as the layer number increases.
        /// </summary>
        public NeutralLayerProps NeutralLayer { get; set; } = new();

    }

    public class NeutralLayerProps
    {
        /// <summary>
        /// Absolute layer colors derived from and starting at baseLayerLuminance.
        /// </summary>
        public string? Layer1 { get; set; }

        /// <summary>
        /// Absolute layer colors derived from and starting at baseLayerLuminance.
        /// </summary>
        public string? Layer2 { get; set; }

        /// <summary>
        /// Absolute layer colors derived from and starting at baseLayerLuminance.
        /// </summary>
        public string? Layer3 { get; set; }

        /// <summary>
        /// Absolute layer colors derived from and starting at baseLayerLuminance.
        /// </summary>
        public string? Layer4 { get; set; }
        /// <summary>
        /// A special layer to support experiences primarily built with cards, especially in light mode, so cards can be white and the container color can be one layer darker.
        /// </summary>
        public string? CardContainer { get; set; }

        /// <summary>
        /// A special layer for floating layers, like flyouts or menus. It will be lighter than any other layers if possible, but will also be white in default light mode, as will neutral layer one.
        /// </summary>
        public string? Floating { get; set; }
    }

    public class FocusStrokeProps
    {
        public string? Inner { get; set; }
        public string? Outer { get; set; }
        public double? Width { get; set; }
    }

    public class RecipeState
    {
        public string? Active { get; set; }
        public string? Focus { get; set; }
        public string? Hover { get; set; }
        public string? Rest { get; set; }
    }

    public class RecipeStateDelta
    {
        public string? Active { get; set; }
        public string? Focus { get; set; }
        public string? Hover { get; set; }
        public string? Rest { get; set; }
        public double? ActiveDelta { get; set; }
        public double? FocusDelta { get; set; }
        public double? HoverDelta { get; set; }
        public double? RestDelta { get; set; }
    }

    public class ElevationShadowProps
    {
        public string? Dialog { get; set; }
        public double? DialogSize { get; set; }
        public string? Flyout { get; set; }
        public double? FlyoutSize { get; set; }
        public string? Tooltip { get; set; }
        public double? TooltipSize { get; set; }

    }

    public class TypeRamps
    {
        /// <summary>
        /// Smallest
        /// </summary>
        public TypeRamp Minus1 { get; set; } = new();

        /// <summary>
        /// Small
        /// </summary>
        public TypeRamp Minus2 { get; set; } = new();

        /// <summary>
        /// Body
        /// </summary>
        public TypeRamp Base { get; set; } = new();

        /// <summary>
        /// Large Level 1
        /// </summary>
        public TypeRamp Plus1 { get; set; } = new();

        /// <summary>
        /// Large Level 2
        /// </summary>
        public TypeRamp Plus2 { get; set; } = new();

        /// <summary>
        /// Large Level 3
        /// </summary>
        public TypeRamp Plus3 { get; set; } = new();

        /// <summary>
        /// Large Level 4
        /// </summary>
        public TypeRamp Plus4 { get; set; } = new();

        /// <summary>
        /// Large Level 5
        /// </summary>
        public TypeRamp Plus5 { get; set; } = new();

        /// <summary>
        /// Large Level 6
        /// </summary>
        public TypeRamp Plus6 { get; set; } = new();
    }

    public class TypeRamp
    {
        public string? FontSize { get; set; }
        public string? FontVariations { get; set; }
        public string? LineHeight { get; set; }
    }
}
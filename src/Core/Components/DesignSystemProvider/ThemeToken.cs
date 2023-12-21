using static System.Net.Mime.MediaTypeNames;

namespace Microsoft.FluentUI.AspNetCore.Components;

/*
 * See https://learn.microsoft.com/en-us/fluent-ui/web-components/getting-started/styling
 * And https://color.fast.design/
 */

public class ThemeToken
{
    public CommonProps Common { get; } = new();
    public ColorsProps Colors { get; } = new();
    public OthersProps Others { get; } = new();

    public class CommonProps
    {
        /// <summary>
        /// Gets or sets a value that may be applied to an element's styles and used as context for child color recipes.
        /// </summary>
        public string? FillColor { get; set; }

        /// <summary>
        /// Set to a custom swatch to use for color recipes for layers and other neutral components.
        /// </summary>
        public string? NeutralBaseColor { get; set; }

        /// <summary>
        /// Used to specify the font string to apply to components. Note that this does not import fonts, so they must either be web standard, assumed to be installed, or imported at the top of your app.
        /// </summary>
        public string? BodyFont { get; set; }

        /// <summary>
        /// Sets the corner radius used by controls with backplates.
        /// Ex: Increase to 6px for slightly rounder buttons and text fields.
        /// </summary>
        public int? ControlCornerRadius { get; set; }

        /// <summary>
        /// Sets the corner radius used layers like cards, flyouts, and dialogs.
        /// Ex: Increase to 20px for very round cards.
        /// </summary>
        public int? LayerCornerRadius { get; set; }

        /// <summary>
        /// Sets the corner radius used layers like cards, flyouts, and dialogs.
        /// Ex: Increase to 20px for very round cards.
        /// </summary>
        public string? Density { get; set; }
    }

    public class ColorsProps
    {
        public RecipeStateful AccentFill { get; } = new();
        public RecipeStateful AccentForeground { get; } = new();
        public RecipeStateful ForegroundOnAccent { get; } = new();
    }

    public class OthersProps
    {
        /// <summary>
        /// Type ramps
        /// </summary>
        public TypeRamps TypeRamps { get; } = new();

        /// <summary>
        ///  This value, multiplied by designUnit, sets the base height of most controls. Works with adaptive density values.
        /// </summary>
        public string? BaseHeightMultiplier { get; set; }

        /// <summary>
        /// This value, multiplied by designUnit, sets the internal horizontal padding of most controls. Works with adaptive density values.
        /// </summary>
        public string? BaseHorizontalSpacingMultiplier { get; set; }

        /// <summary>
        /// The unit size of the design grid. Used to calculate height and spacing sizes for controls.
        /// </summary>
        public string? DesignUnit { get; set; }

        /// <summary>
        /// Controls the width of the stroke of a component that has a stroke.
        /// </summary>
        public string? StrokeWidth { get; set; }

        /// <summary>
        /// Controls with width of the stroke of a component that has a stroke when it has document focus.
        /// </summary>
        public string? FocusStrokeWidth { get; set; }

        /// <summary>
        /// The opacity of disabled controls. Careful with values that are too high as the control may no longer look disabled. There are no contrast requirements for a disabled control.
        /// </summary>
        public string? DisabledOpacity { get; set; }
    }

    public class TypeRamps
    {
        /// <summary>
        /// Smallest
        /// </summary>
        public TypeRamp Minus1 { get; } = new();

        /// <summary>
        /// Small
        /// </summary>
        public TypeRamp Minus2 { get; } = new();

        /// <summary>
        /// Body
        /// </summary>
        public TypeRamp Base { get; } = new();

        /// <summary>
        /// Large Level 1
        /// </summary>
        public TypeRamp Plus1 { get; } = new();

        /// <summary>
        /// Large Level 2
        /// </summary>
        public TypeRamp Plus2 { get; } = new();

        /// <summary>
        /// Large Level 3
        /// </summary>
        public TypeRamp Plus3 { get; } = new();

        /// <summary>
        /// Large Level 4
        /// </summary>
        public TypeRamp Plus4 { get; } = new();

        /// <summary>
        /// Large Level 5
        /// </summary>
        public TypeRamp Plus5 { get; } = new();

        /// <summary>
        /// Large Level 6
        /// </summary>
        public TypeRamp Plus6 { get; } = new();
    }

    public class TypeRamp
    {
        public string? FontSize { get; set; }
        public string? LineHeight { get; set; }
    }

    public class RecipeStateful
    {
        public string? Active { get; set; }
        public string? Focus { get; set; }
        public string? Hover { get; set; }
        public string? Recipe { get; set; }
        public string? Rest { get; set; }
        public string? ActiveDelta { get; set; }
        public string? FocusDelta { get; set; }
        public string? HoverDelta { get; set; }
        public string? RestDelta { get; set; }
    }
}
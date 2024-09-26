## Design Token support
 
The Fluent UI Blazor Components enable design customization and personalization, while automatically maintaining accessibility. This is
accomplished through setting various "Design Tokens". In total there are over 160 distinct design tokens defined in the library and you can
use all of these from Blazor, both from C# code as in a declarative way in your `.razor` pages.

See <a href="https://learn.microsoft.com/en-us/fluent-ui/web-components/design-system/design-tokens" target="_blank">https://learn.microsoft.com/en-us/fluent-ui/web-components/design-system/design-tokens</a> for more 
information on how Design Tokens are implemented in the web components script.

## Styling web components with Design Tokens

Many of the Fluent UI Blazor components wrap the web components which are designed to be stylistically flexible, allowing dramatic changes to visual design with minimal code changes. This is possible through the extensive use of Design Tokens and an [adaptive color system](#adaptive-color-system).

### Fluent UI Web Components Design Tokens

The following Design Tokens can be used to configure components stylistically.

#### Light and dark mode

The most common need for setting a token is to switch between light and dark mode.

- `BaseLayerLuminance`: Set to `StandardLuminance.DarkMode` to switch into dark mode.

This is a decimal value, and the `LightMode` and `DarkMode` constants represent the standard points for light and dark mode. You could set it to any value `0` (black) to `1` (white) depending on your needs.
>**Note:** To get the corresponding value from the `StandardLuminance` enum, use the `GetLuminanceValue()` extension method. 

#### Layers and fill color

The second most common need for manually applying color is to define layers. When you adjust `BaseLayerLuminance` as above, you're actually adjusting the `NeutralLayer*` recipe colors.

- `FillColor`: Sets a value that *may* be applied to an element's styles and used as context for child color recipes. The default value is `NeutralLayer1`.
*Ex: Set to `NeutralLayer2` for a 'lower' container, like beneath a Card or Accordion.*

The details: Avoid setting this to a fixed color value. The scenario above works because the neutral layer recipe colors *come from* the neutral palette. The `FillColor` token is used by most color recipes only as a *reference* for the *luminance* (or brightness) context. That's because the recipes are still drawing from their palette, and setting the `FillColor` does not *change* the palette.

#### Adjust neutral or accent colors

- `NeutralBaseColor`: Set to a custom swatch to use for color recipes for layers and other neutral components.
*Ex: `SwatchRGB.from(parseColorHexRGB('#A90000')!)`*
- `AccentBaseColor`: Set to a custom swatch to use for color recipes for accent buttons, checkboxes, etc.

#### Typography

- `BodyFont`: Used to specify the font string to apply to components. Note that this does not import fonts, so they must either be web standard, assumed to be installed, or imported at the top of your app.

These tokens and values represent the default Fluent type ramp. The tokens should be used and adjusted relatively. For instance, if the type should be larger overall, increase the size of the entire type ramp instead of restyling a component to use "Plus 1" instead of "base".

| Level | Font Size Token Name | Line Height Token Name |
| --- | --- | --- |
| Minus 2 (smallest) | `TypeRampMinus2FontSize` | `TypeRampMinus2LineHeight` |
| Minus 1 | `TypeRampMinus1FontSize` | `TypeRampMinus1LineHeight` |
| Base (body) | `TypeRampBaseFontSize` | `TypeRampBaseLineHeight` |
| Plus 1 | `TypeRampPlus1FontSize` | `TypeRampPlus1LineHeight` |
| Plus 2 | `TypeRampPlus2FontSize` | `TypeRampPlus2LineHeight` |
| Plus 3 | `TypeRampPlus3FontSize` | `TypeRampPlus3LineHeight` |
| Plus 4 | `TypeRampPlus4FontSize` | `TypeRampPlus4LineHeight` |
| Plus 5 | `TypeRampPlus5FontSize` | `TypeRampPlus5LineHeight` |
| Plus 6 (largest) | `TypeRampPlus6FontSize` | `TypeRampPlus6LineHeight` |

* * *

#### Sizing

Here are the common sizing tokens you may want to adjust:

- `ControlCornerRadius`: Sets the corner radius used by controls with backplates.
*Ex: Increase to `6px` for slightly rounder buttons and text fields.*
- `LayerCornerRadius`: Sets the corner radius used layers like cards, flyouts, and dialogs.
*Ex: Increase to `20px` for very round cards.*
- `Density` (in process): A modifier used with sizing tokens `BaseHeightMultiplier` and `BaseHorizontalSpacingMultiplier`.
*Ex: Set to `1` to increase control size or `-1` to decrease.*

These are less common and more nuanced:

- `BaseHeightMultiplier`: This value, multiplied by `designUnit`, sets the base height of most controls. Works with adaptive `density` values.
- `BaseHorizontalSpacingMultiplier` (future): This value, multiplied by `DesignUnit`, sets the internal horizontal padding of most controls. Works with adaptive `density` values.
- `DesignUnit`: The unit size of the design grid. Used to calculate height and spacing sizes for controls.

#### Misc.

Common:

- `Direction`: The primary document direction (LTR or RTL).

Less common:

- `StrokeWidth`: Controls the width of the stroke of a component that has a stroke.
- `FocusStrokeWidth`: Controls with width of the stroke of a component that has a stroke when it has document focus.
- `DisabledOpacity`: The opacity of disabled controls. Careful with values that are too high as the control may no longer look disabled. There are no contrast requirements for a disabled control.

### Adaptive color system

The design tokens are built around an adaptive color system that provides some unique advantages:

- Ensure text meets [WCAG](https://www.w3.org/WAI/standards-guidelines/wcag/) contrast requirements.
- Easily swap from light mode to dark, or anywhere in-between.
- Color theming through palette tinting.
- Perceptually uniform UI across background colors.

To accomplish these goals, the web components make heavy use of algorithmic colors called Recipes. Recipes are a combination of an algorithm and input values that produce a desired result. Just as you can bake different types of cookies with different combinations of sugar, butter, flour, and salt, you can produce different design system treatments by altering recipe values (measurements) or algorithms (instructions).

The current base recipes are closely related to their algorithm, but that's a convention and not a requirement. What follows is a list of the algorithms, which function on like-named values. For instance, `AccentFill` relies on `AccentFillRestDelta`, `AccentFillHoverDelta`, `AccentFillActiveDelta`, and `AccentFillFocusDelta`.

Recipes are currently used for color values, but they are not limited to that and their usage will be expanded soon.


#### Common functionality

Most color recipes are based on a `palette`. Currently there is built-in support for `accent` and `neutral` palettes.

**For Design Tokens that work with a color value, you must call the `ToSwatch()` extension method on a string value or use one of the Swatch constructors. This makes sure the color is using a format that Design Tokens can handle. A Swatch has a lot of commonality with the `System.Drawing.Color` struct. Instead of the values of the components being between 0 and 255, in a Swatch the components are expressed as a value between 0 and 1.**

This is a core concept of Adaptive UI which allows the recipes to vary based on the containing component's color. For instance, supporting a button with consistent treatment between light and dark modes is done with a single recipe.

Many recipes are "stateful", meaning they support rest, hover, active, and focus states for a component.

**"Fill"** means the recipe is intended to fill a larger area, commonly like a component backplate.

**"Foreground"** means the recipe is intended for text, icons, or other lightweight decorations where you need or want to meet contrast requirements.

**"Stroke"** means the recipe is intended for lines, either outline or divider.

#### Accent algorithms

##### AccentFill

Stateful.

Relies on `textColor` and `contrastTarget` to find the closest colors from the supplied palette that can be used for component states. For instance, colors needed to support white text and a 14px font (which requires 4.5:1 contrast).

##### AccentForeground

Stateful.

Commonly for link text or icon. Also for smaller elements that might not show up well using `accentFill`, for instance if your accent color is dark purple and you support a dark mode interface.

Like `accentFill` this relies on `textColor` and `contrastTarget` to find the closest colors from the supplied palette that can be used for component states.

##### ForegroundOnAccent

Not stateful.

Technically this doesn't *use* the accent palette, but it's designed to be used *over* the accent palette. This algorithm simply returns black or white based on the provided `contrastTarget`. It returns white if possible, as a common treatment for an accent button is white text over the accent color.

#### Neutral algorithms

##### NeutralDivider

Not stateful.

Used for decorative dividers that do not need to meet contrast requirements.

##### NeutralFill

Stateful.

The most basic fill used for buttons or other components.

##### NeutralFillContrast

Stateful.

Often Used as a selected state or anywhere you want to draw attention. Meets contrast requirements with the containing background.

##### NeutralFillInput

Stateful.

Another basic fill, applied to input elements to allow easy differentiation from other components like buttons.

##### NeutralFillStealth

Stateful.

More subtle than `neutralFill` in that the resting state is transparent. Often used for low-priority features to draw less attention.

##### NeutralForeground

Not stateful.

Most common recipe, used for plain text or icons.

##### NeutralForegroundHint

Not stateful.

Used for subtle text. Meets 4.5:1 minimum contrast requirement.

##### NeutralStroke

Stateful.

Used for strong outline, either alone or with a fill.

#### Layers

The layer recipes are used for different sections of an app or site. They are designed to be able to stack, but that is not required. When stacked in sequence, the layers will lighten on top of each other.

The key feature of layering is to support the primary container color for light or dark mode. This produces absolute colors based on the `BaseLayerLuminance` value, which sets the luminance for layer one. This is any value between 0 for black or 1 for white.

The difference between each layer is defined with `NeutralFillLayerRestDelta`.

Layers are not stateful.

##### NeutralFillLayer

The only layer recipe that's relative to the container color instead of absolute. The most common example of this is a Card, which will be one layer color lighter than its container.

##### NeutralLayer1, NeutralLayer2, NeutralLayer3, and NeutralLayer4

Absolute layer colors derived from and starting at `BaseLayerLuminance`. Layer one is lightest and the values darken as the layer number increases.

##### NeutralLayerCardContainer

A special layer to support experiences primarily built with cards, especially in light mode, so cards can be white and the container color can be one layer darker.

##### NeutralLayerFloating

A special layer for floating layers, like flyouts or menus. It will be lighter than any other layers if possible, but will also be white in default light mode, as will neutral layer one.




### Using Design Tokens from code


> **Important**
> 
> 
> **The Design Tokens are manipulated through JavaScript interop working with an `ElementReference`. There is no JavaScript element until after the component 
is rendered. This means you can only work with the Design Tokens from code after the component has been rendered in `OnAfterRenderAsync` and not in any earlier 
lifecycle methods**.

There are a couple of methods available **per design token** to get or set its value:
- `{DesignTokenName}.SetValueFor(ElementReference element, T value)` - Sets the value for the given element.
- `{DesignTokenName}.DeleteValueFor(ElementReference element)` - Deletes the value for the given element.
- `{DesignTokenName}.WithDefault(T value)` - Sets the default value for the whole design system use.
- `{DesignTokenName}.GetValueFor(ElementReference element)` - Gets the value for the given element.- `

#### Example
Given the following `.razor` page fragment:

```cshtml
<FluentButton @ref="ref1">A button</FluentButton>
<FluentButton @ref="ref2" Appearance.Accent>Another button</FluentButton>
<FluentButton @ref="ref3">And one more</FluentButton>
<FluentButton @ref="ref4" @onclick=OnClick>Last button</FluentButton>
```

You can use Design Tokens to manipulate the styles from C# code as follows:

```csharp
@using Microsoft.FluentUI.AspNetCore.Components.DesignTokens

[Inject]
private BaseLayerLuminance BaseLayerLuminance { get; set; } = default!;

[Inject]
private AccentBaseColor AccentBaseColor { get; set; } = default!;

[Inject]
private BodyFont BodyFont { get; set; } = default!;

[Inject]
private StrokeWidth StrokeWidth { get; set; } = default!;

[Inject]
private ControlCornerRadius ControlCornerRadius { get; set; } = default!;

private FluentButton? ref1;
private FluentButton? ref2;
private FluentButton? ref3;
private FluentButton? ref4;

protected override async Task OnAfterRenderAsync(bool firstRender)
{
	if (firstRender)
	{
		//Set to dark mode
		await BaseLayerLuminance.SetValueFor(ref1!.Element, (float)0.15);

		//Set to Excel color
		await AccentBaseColor.SetValueFor(ref2!.Element, "#217346".ToSwatch());

		//Set the font
		await BodyFont.SetValueFor(ref3!.Element, "Comic Sans MS");

		//Set 'border' width for ref4
		await StrokeWidth.SetValueFor(ref4!.Element, 7);
		//And change conrner radius as well
		await ControlCornerRadius.SetValueFor(ref4!.Element, 15);

		// If you would like to change the BaseLayerLuminance  value for the whole site, you can use the WithDefault method
		await BaseLayerLuminance.WithDefault((float)0.15);

		StateHasChanged();
	}
}

public async Task OnClick()
{
	//Remove the wide border
	await StrokeWidth.DeleteValueFor(ref4!.Element);
}
```

As can be seen in the code above (with the `ref4.Element`), it is possible to apply multiple tokens to the same component.
 
For Design Tokens that work with a color value, you must call the `ToSwatch()` extension method on a string value or use one of the Swatch constructors. This 
makes sure the color is using a format that Design Tokens can handle. A Swatch has a lot of commonality with the `System.Drawing.Color` struct. Instead of 
the values of the components being between 0 and 255, in a Swatch the components are expressed as a value between 0 and 1.

### Using Design Tokens as components
The Design Tokens can also be used as components in a `.razor` page directely. It looks like this:

```html
<BaseLayerLuminance Value="(float?)0.15">
	<FluentCard ParentReference="@context">
		<div class="contents">
			Dark
			<FluentButton Appearance="Appearance.Accent">Accent</FluentButton>
			<FluentButton Appearance="Appearance.Stealth">Stealth</FluentButton>
			<FluentButton Appearance="Appearance.Outline">Outline</FluentButton>
			<FluentButton Appearance="Appearance.Lightweight">Lightweight</FluentButton>
		</div>
	</FluentCard>
</BaseLayerLuminance>
```

To make this work, a link needs to be created between the Design Token component and its child components. This is done with the `ParentReference="@context"` construct. 

> **Note**
> 
> Only one Design Token component at a time can be used this way. If you need to set more tokens, use the code approach as described in Option 1 above.


## Colors for integration with specific Microsoft products
If you are configuring the components for integration into a specific Microsoft product, the following table provides `AccentBaseColor` values you can use. 
*The specific accent colors for many Office applications are offered in the `OfficeColor` enumeration.*

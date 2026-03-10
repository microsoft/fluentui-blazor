---
title: Themes
route: /Theme/[Default]
icon: PaintBrushSparkle
---


# Theme

## Default Theme and Brand color

A Fluent UI Theme is represented by a set of tokens. Each token resolves to a single value which can be assigned to a CSS property.

The Fluent UI Design System comes with a default theme which provides a modern and cohesive look and feel across all components. It includes a set of
predefined colors, typography, and spacing that can be easily customized to fit your brand. In this section, the focus is on the
color(s).
Besides the default theme, dark and light versions of the Teams theme are also included. This is especially useful if you use the Fluent UI
Blazor library for building Teams add-ins or applications

In the Fluent Design System, there is one primary color defined which is named the **Brand** color. The brand color is used to represent the identity of
a product or organization and is used in various UI elements such as buttons, links, and highlights to create a consistent visual identity.

The brand color is used to generate a 16-color ramp that passes contrast checks for accessibility. This 16-color ramp includes a range of shades
and tints of the brand color, allowing for flexibility in design while maintaining a cohesive look. The components in the library use this generated ramp
of colors in their presentation.

When nothing is configured, the library will use a default brand color, which is a shade of blue (#0F6CBD). This default color is chosen to provide a
visually appealing and accessible experience for users. 

## Customizing the Brand color
The Fluent Design system allows for customizing the brand color to align with your brand identity. By providing a custom brand color, you can create a unique
and personalized experience for your users while still adhering to the principles of the Fluent Design System.

The Fluent UI Blazor library includes built-in support for customizing the brand color and generating a corresponding color ramp. This uses the _exact same
algorithms_ as the [Fluent Theme Designer](https://storybooks.fluentui.dev/react/?path=/docs/theme-theme-designer--docs), a tool provided by the Fluent UI
React team at Microsoft to help designers and developers create custom themes based on the Fluent Design System. By using these same algorithms, we ensure
the colors generated in your Blazor application will match exactly what you see in the Theme Designer.

You can choose any valid hex color code as your brand color, and the library will automatically generate and apply the appropriate shades and tints to ensure
accessibility and visual consistency.

### Using an exact color in the ramp
We've added functionality which allows you to specify that the generated ramp should include the exact specified color as one of the colors in the ramp. This
gives you more control over the generated colors and ensures that the key color is included in the ramp. This can be particularly useful if you want to ensure
that the exact brand color is used in certain UI elements while still benefiting from the generated shades and tints for other elements.

>[!WARNING] When using the exact mode, there is no guarantee **_all_** colors in the generated ramp will pass contrast checks for accessibility.

The choice of using a dark or a light theme mode determines which color(s) in the ramp will use the exact specified color. Technically, choosing to use an
exact color will determine what values are assigned to the `--colorBrandBackground` and `--colorCompoundBrandBackground` CSS variables.

We offer two ways to set a custom brand color in your Blazor application:

### Set the Brand color declaratively
You can add a `data-theme` attribute to the `<body>` tag in your HTML and set its value to 'light', 'dark', or 'system' to specify the theme mode. This allows
you to configure the theme mode.

You can add a `data-theme-color` attribute to the `<body>` tag in your HTML and set its value to a valid hex color code (e.g., #FF0000). The library will
automatically detect this attribute, generate a color ramp based on the provided color, and apply it to the application.

The declarative `data-theme-*` attributes are treated as developer-provided overrides and are **_not_** persisted to `localStorage`.

To have acces to all variables that are available to customize a theme, you can use the methods described below.

### Set the Brand color with code
A full API is available for configuring the theme programmatically in your Blazor application. This allows you to dynamically change the theme based on user
interactions or other conditions in your application.

The following methods are available for setting the brand color programmatically:

| Name | Description |
|---|---|
| `CreateCustomThemeAsync(string color, double hueTorsion, double vibrancy, bool isDark, bool isExact = false)` | Creates a Theme which can then be applied to the application by passing it to the `SetThemeAsync` method. This allows you to generate a custom theme based on specific parameters and have full control over all the tokens. |
| `SetThemeAsync(ThemeType type)` | Where `ThemeType` is an enum with the following values: `Default`, or `Teams`. Uses the effective mode (light or dark). |
| `SetThemeAsync(ThemeType type, ThemeMode mode)` | Where `ThemeType` is an enum with the following values: `Default`, or `Teams`, and `ThemeMode` is an enum with the following values: `Light`, `Dark`, or `System`. |
| `SetThemeAsync(string color, bool isExact = false)` | Where `color` should be a valid hex color code (e.g., `#FF0000`), and `isExact` controls whether the exact provided color is used for the brand background. Uses the effective mode (light or dark). |
| `SetThemeAsync(string color, double hueTorsion, double vibrancy, ThemeMode mode, bool isExact = false)` | Control every aspect of the theme generation by providing the brand color, hue torsion, vibrancy, theme mode and whether to use the exact provided color in the ramp. Where `color` must be a valid hex color code (e.g., `#FF0000`), `hueTorsion` needs to be a number between `-0.5` and `0.5`, `vibrancy` needs to be a number between `-0.5` and `0.5`. |
| `SetThemeAsync(IReadOnlyDictionary<string, string> theme)` | Where the `theme` parameter is a Theme (created with `CreateCustomThemeAsync`), with token names as keys and their corresponding values as values. This allows you to apply a fully custom theme. |
| `ClearThemeSettingsAsync()` | Removes any stored theme configuration from `localStorage` and resets the theme to the default settings. |
| `IsSystemDarkAsync()` | Boolean method that checks if the user's system preference is set to dark mode. |
| `IsDarkModeAsync()` | Boolean method that checks if the current effective theme mode is dark mode. |
| `GetColorRampAsync()` | Returns the current, cached, color ramp as a dictionary ramp number and corresponding color values. |
| `GetColorRampFromSettingsAsync(ThemeSettings settings)` | Generates a new color ramp based on the provided settings. Validates inputs and always recalculates the ramp without using the internal cache. Returns `null` for invalid inputs. |
| `SwitchDirectionAsync()` | Change the effective theme direction between left-to-right (LTR) and right-to-left (RTL). This is particularly useful for supporting languages that are read from right to left, such as Arabic or Hebrew. |
| `SwitchThemeAsync()` | Switches the effective theme mode between light and dark. This is particularly useful for allowing users to toggle between light and dark themes based on their preferences. |

The SetThemeAsync result is cached in `localStorage` so that the theme configuration can be persisted across sessions and restored on subsequent visits
to the application. The only exception to this is when using the `SetThemeAsync(IReadOnlyDictionary<string, string> theme)` overload, which applies a
fully custom theme without caching it.



## Theme Designer
Use the designer below to experiment with different brand colors and see how they affect the generated color ramp. You can adjust the hue, vibrancy,
and theme mode to create a custom theme that fits your brand identity. Hue Torsion and Vibrancy are defined here to be between -50 and 50, but the underlying
implementation uses a range of -0.5 to 0.5, so the values you input will be divided by 100 before being applied to the color generation algorithm.

{{ ThemeDesigner }}



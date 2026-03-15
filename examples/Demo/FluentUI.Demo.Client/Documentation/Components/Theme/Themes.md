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
| `CreateCustomThemeAsync(ThemeSettings settings)` | Creates a custom `Theme` based on the specified settings. The returned theme can be modified by the caller before it is applied. |
| `SetThemeAsync(Theme theme)` | Applies a fully custom `Theme` (for example created with `CreateCustomThemeAsync`). This applies the theme without generating it from brand settings. |
| `SetThemeAsync(ThemeType type)` | Sets a built-in theme by type (`Default` or `Teams`) using the current effective mode (light or dark). |
| `SetThemeAsync(ThemeType type, ThemeMode mode)` | Sets a built-in theme by type (`Default` or `Teams`) and a specific mode (`Light`, `Dark`, or `System`). |
| `SetThemeAsync(string color, bool isExact = false)` | Sets a custom brand theme from a hex color (e.g. `#FF0000`). `isExact` controls whether the exact provided color is used for the brand background. Uses `ThemeMode.System`. |
| `SetThemeAsync(ThemeSettings settings)` | Sets a custom brand theme using full settings control: brand color, hue torsion (`-0.5` to `0.5`), vibrancy (`-0.5` to `0.5`), mode, and exact color behavior. |
| `SetLightThemeAsync()` | Convenience wrapper that sets the `Default` theme to `Light` mode. |
| `SetDarkThemeAsync()` | Convenience wrapper that sets the `Default` theme to `Dark` mode. |
| `SetSystemThemeAsync()` | Convenience wrapper that sets the `Default` theme to `System` mode. |
| `SetTeamsLightThemeAsync()` | Convenience wrapper that sets the `Teams` theme to `Light` mode. |
| `SetTeamsDarkThemeAsync()` | Convenience wrapper that sets the `Teams` theme to `Dark` mode. |
| `SetTeamsSystemThemeAsync()` | Convenience wrapper that sets the `Teams` theme to `System` mode. |
| `ClearThemeSettingsAsync()` | Removes any stored theme configuration from `localStorage` and resets the theme to the default settings. |
| `IsSystemDarkAsync()` | Returns whether the user's system preference is set to dark mode. |
| `IsDarkModeAsync()` | Returns whether the current effective theme mode is dark mode. |
| `GetColorRampAsync()` | Returns the current cached color ramp as a dictionary of ramp number to color value, or `null` if no custom ramp has been generated yet. |
| `GetColorRampFromSettingsAsync(ThemeSettings settings)` | Generates a new color ramp based on the provided settings. Validates inputs and always recalculates the ramp without using the internal cache. Returns `null` for invalid inputs. |
| `SwitchDirectionAsync()` | Switches the document direction between left-to-right (LTR) and right-to-left (RTL). |
| `SwitchThemeAsync()` | Toggles between light and dark mode. Returns `true` if the new effective theme is dark. |
| `SetThemeToElementAsync(ElementReference element, ThemeSettings settings)` | Applies a custom brand theme (from settings) to a specific element only; does not change the global theme. |

The SetThemeAsync result is cached in `localStorage` so that the theme configuration can be persisted across sessions and restored on subsequent visits
to the application. The only exception to this is when using the `SetThemeAsync(Theme theme)` overload, which applies a
fully custom theme without caching it.


## Theme Designer
Use the designer below to experiment with different brand colors and see how they affect the generated color ramp. You can adjust the hue, vibrancy,
and theme mode to create a custom theme that fits your brand identity. Hue Torsion and Vibrancy are defined here to be between -50 and 50, but the underlying
implementation uses a range of -0.5 to 0.5, so the values you input will be divided by 100 before being applied to the color generation algorithm.

{{ ThemeDesigner }}


## Create and alter a Theme

In addition to setting the brand color and generating a theme from it, you can also create a `Theme` object directly in your code and modify its properties
before applying it. This gives you full control over all aspects of the theme, including colors, typography, spacing, and more.

Clicking the button below applies an altered theme that changes the border radius, color, font family, and line height to the components in this part of the page only.

{{ CreateAndAlterTheme Files=Razor:CreateAndAlterTheme.razor;Code:CreateAndAlterTheme.razor.cs }}

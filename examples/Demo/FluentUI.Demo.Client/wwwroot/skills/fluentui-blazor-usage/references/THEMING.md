# Theming — Fluent UI Blazor v5

## Overview

v5 uses a **CSS custom property** model backed by `@fluentui/web-components` v3
and `@fluentui/tokens`. There is **no `FluentDesignTheme` component** — themes
are applied via JavaScript and tracked by the `body[data-theme]` attribute.

## How It Works

1. The `@fluentui/web-components` library provides a `setTheme()` function
2. Calling `setTheme(theme)` sets CSS custom properties on the document
3. The `body[data-theme]` attribute tracks light/dark mode for CSS selectors
4. All Fluent UI components read their styles from these CSS variables

## Applying Themes

### Set initial theme (automatic)

The library auto-detects system preference on load if no `data-theme` attribute
is set on `<body>`.

### Set theme explicitly

```html
<!-- In your HTML, before components load -->
<body data-theme="dark">
```

Or via JS interop from Blazor:

```csharp
@inject IJSRuntime JSRuntime

// Set light theme
await JSRuntime.InvokeVoidAsync("Blazor.theme.setLightTheme");

// Set dark theme
await JSRuntime.InvokeVoidAsync("Blazor.theme.setDarkTheme");

// Toggle between light and dark (returns bool: true = dark)
var isDark = await JSRuntime.InvokeAsync<bool>("Blazor.theme.switchTheme");

// Apply default (respects body attribute or system preference)
await JSRuntime.InvokeVoidAsync("Blazor.theme.setDefaultTheme");
```

### Detect current mode

```csharp
var isDark = await JSRuntime.InvokeAsync<bool>("Blazor.theme.isDarkMode");
var isSystemDark = await JSRuntime.InvokeAsync<bool>("Blazor.theme.isSystemDark");
```

## Theme-Conditional CSS Classes

The library provides utility classes to show/hide content based on theme:

```razor
@* Only visible in dark mode *@
<span class="hidden-if-light">Dark mode content</span>

@* Only visible in light mode *@
<span class="hidden-if-dark">Light mode content</span>
```

Common pattern for theme toggle button:

```razor
<FluentButton Appearance="ButtonAppearance.Transparent"
              OnClick="@(async () => await JSRuntime.InvokeVoidAsync("Blazor.theme.switchTheme"))">
    <FluentIcon Class="hidden-if-light"
                Value="@(new Icons.Filled.Size20.WeatherSunny())" />
    <FluentIcon Class="hidden-if-dark"
                Value="@(new Icons.Filled.Size20.WeatherMoon())" />
</FluentButton>
```

## CSS Custom Properties / Design Tokens

All Fluent design tokens are CSS custom properties. Use them in component
styles or CSS files:

### Colors

```css
/* Neutral palette */
var(--colorNeutralForeground1)
var(--colorNeutralForeground2)
var(--colorNeutralBackground1)
var(--colorNeutralBackground2)
var(--colorNeutralStroke1)

/* Brand palette */
var(--colorBrandBackground)
var(--colorBrandForeground1)
var(--colorBrandStroke1)
var(--colorNeutralForegroundOnBrand)

/* Status colors */
var(--colorPaletteRedBackground3)
var(--colorPaletteGreenForeground1)
var(--colorPaletteYellowBackground3)
```

### Typography

```css
var(--fontFamilyBase)
var(--fontFamilyMonospace)
var(--fontFamilyNumeric)
var(--fontSizeBase100)  /* 10px */
var(--fontSizeBase200)  /* 12px */
var(--fontSizeBase300)  /* 14px - body text */
var(--fontSizeBase400)  /* 16px */
var(--fontSizeBase500)  /* 20px */
var(--fontSizeBase600)  /* 24px */
var(--fontWeightRegular)
var(--fontWeightSemibold)
var(--fontWeightBold)
var(--lineHeightBase300)
```

### Spacing & Borders

```css
var(--spacingHorizontalS)
var(--spacingHorizontalM)
var(--spacingVerticalS)
var(--spacingVerticalM)
var(--borderRadiusSmall)
var(--borderRadiusMedium)
var(--borderRadiusLarge)
var(--borderRadiusCircular)
var(--strokeWidthThin)
var(--strokeWidthThick)
```

### Shadows

```css
var(--shadow2)
var(--shadow4)
var(--shadow8)
var(--shadow16)
var(--shadow28)
var(--shadow64)
```

### Animations

```css
var(--durationUltraFast)  /* 50ms */
var(--durationFaster)     /* 100ms */
var(--durationFast)       /* 150ms */
var(--durationNormal)     /* 200ms */
var(--durationSlow)       /* 300ms */
var(--curveEasyEase)
var(--curveDecelerateMax)
var(--curveAccelerateMax)
```

## C# Style Constants

Instead of writing raw CSS variable strings, use the typed C# constants:

### StylesVariables

```csharp
// Typography
StylesVariables.Fonts.Size.Base300     // "var(--fontSizeBase300)"
StylesVariables.Fonts.Weight.Bold      // "var(--fontWeightBold)"
StylesVariables.Fonts.Family.Base      // "var(--fontFamilyBase)"

// Borders
StylesVariables.Borders.Radius.Medium  // "var(--borderRadiusMedium)"
StylesVariables.Strokes.Width.Thin     // "var(--strokeWidthThin)"

// Shadows  
StylesVariables.Shadows.Shadows16      // "var(--shadow16)"

// Animation
StylesVariables.Durations.Fast         // "var(--durationFast)"
StylesVariables.Curves.EasyEase        // "var(--curveEasyEase)"
```

### SystemColors

```csharp
// Neutral
SystemColors.Neutral.Foreground1       // "var(--colorNeutralForeground1)"
SystemColors.Neutral.Background1       // "var(--colorNeutralBackground1)"
SystemColors.Neutral.Stroke1           // "var(--colorNeutralStroke1)"
SystemColors.Neutral.ForegroundOnBrand // "var(--colorNeutralForegroundOnBrand)"

// Brand
SystemColors.Brand.Background          // "var(--colorBrandBackground)"
SystemColors.Brand.Foreground1         // "var(--colorBrandForeground1)"

// Status / Alerts
SystemColors.Alerts.Success            // Success background color
SystemColors.Alerts.Warning            // Warning background color
SystemColors.Alerts.Error              // Error background color

// Presence indicators
SystemColors.Presence.Available        // Available color
SystemColors.Presence.Busy             // Busy color
SystemColors.Presence.Away             // Away color
```

### CommonStyles (pre-built style strings)

```csharp
CommonStyles.NeutralBorder1        
// "border: var(--strokeWidthThin) solid var(--colorNeutralStroke1);"

CommonStyles.BrandBackground       
// "background-color: var(--colorBrandBackground); color: var(--colorNeutralForegroundOnBrand);"

CommonStyles.NeutralBackground     
// "background-color: var(--colorNeutralBackground1); color: var(--colorNeutralForeground1);"

CommonStyles.NeutralBorderShadow4  
// "box-shadow: var(--shadow4);"
```

## Spacing Utilities

v5 includes a CSS spacing system with utility classes for margin and padding.
Use them via the `Margin` and `Padding` parameters available on every component:

```razor
<FluentCard Margin="@Margin.All4" Padding="@Padding.Horizontal3">
    Content with 16px margin and 12px horizontal padding
</FluentCard>

<FluentButton Margin="@Margin.Top2 @Margin.Bottom2">
    Button with vertical margin
</FluentButton>
```

Spacing scale: 1 = 4px, 2 = 8px, 3 = 12px, 4 = 16px, 5 = 20px, etc.

Directions: `All`, `Top`, `Bottom`, `Left`, `Right`, `Horizontal`, `Vertical`

Example values: `Margin.Top2` → class `mt-2` → `margin-top: 8px`

---
title: Theme / System colors
route: /theme/colors
---

# System colors

We include a wide range of constants for FluentUI **CSS variables** and **colors** to use
in your application.

## How it works

Each CSS variable defined in the `FluentUI` are available
using an equivalent .NET constants.

1. **StylesVariables** namespace contains a list of all classes which
   are used to define the constants.  
   E.g. `StylesVariables.Fonts.Family.Monospace = "var(--fontFamilyMonospace)"`.

1. **SystemColors** namespace contains a list of all constants
   which are used to define the CSS **Color** variables.  
   E.g. `SystemColors.Brand.Background = "var(--colorBrandBackground)"`.

```razor
<div style="background: @(SystemColors.Brand.Background); color: @(SystemColors.Neutral.ForegroundOnBrand);">
    <div style="font-family: @(StylesVariables.Fonts.Family.Monospace);">
        Hello World
    </div>
</div>
```

{{ ColorsDefault SourceCode=false }}

<br /><br /><br />

## `SystemColors` Constants

{{ SystemColorsTable SourceCode=false }}

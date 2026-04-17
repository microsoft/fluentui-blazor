---
title: CSS Naming and Isolation in Blazor
impact: MEDIUM-HIGH
impactDescription: Generic or unscoped CSS class names cause style conflicts across components and with consuming applications
tags: blazor, css, isolation, deep, naming, attributes
---

## CSS Isolation

Write styles in a sub-css file: `[Component].razor.css`. Each component gets a scope identifier (`b-[string]`) for isolation.

### Child Component Styles

Use `::deep` [pseudo-element](https://developer.mozilla.org/docs/Web/CSS/Pseudo-elements) to style descendant elements in child components:

```css
::deep h1 {
    color: red;
}
```

> `::deep` only works for **child components**. Add a global `<div>` in your component to define the child component scope.

### CSS Class Naming Strategy

Use a **root class + attribute selectors** approach rather than class-name prefixing (BEM-style) or generic class names.

#### Rules

1. **One namespaced root class per component** — The outermost element of each component must carry a unique, project-prefixed class (e.g. `.my-component`). All descendant styles are scoped under this root class.

2. **Use HTML attributes as styling hooks** — Instead of inventing extra CSS classes for variants, states, or layout options, add semantic attributes to the rendered HTML and target them with attribute selectors. Attributes mirror component parameters, keeping markup declarative and self-documenting.

3. **Never use generic, unprefixed class names** — Class names like `.grid`, `.column-header`, `.hover`, or `.column` are too broad and risk collisions with consuming applications or other libraries.

#### Correct — root class + attribute selectors

```css
/* Root class scopes the component */
.my-component {
    display: grid;
}

/* Attributes reflect component parameters */
.my-component[spacing="3"] {
    gap: 12px;
}

/* Child elements also targeted via attributes under the root */
.my-component > div[role="header"] {
    font-weight: bold;
}

.my-component > div[align="center"] {
    text-align: center;
}
```

#### Wrong — generic or BEM-style class explosion

```css
/* Too generic: will collide with other libraries or user styles */
.grid { display: grid; }
.column-header { font-weight: bold; }
.hover { background: highlight; }
.col-justify-start { text-align: start; }

/* BEM-style: verbose and duplicates parameter semantics */
.my-component-spacing-3 { gap: 12px; }
.my-component-header-bold { font-weight: bold; }
```

#### Why

| Concern | Root + Attributes | Generic classes | BEM prefixing |
|---------|:-:|:-:|:-:|
| Collision-safe | Yes | **No** | Yes |
| Lean markup | Yes | Yes | **No** |
| Self-documenting | Yes | **No** | Partially |
| Maps to component API | Yes | **No** | **No** |

Reference: [Blazor CSS Isolation](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/css-isolation)

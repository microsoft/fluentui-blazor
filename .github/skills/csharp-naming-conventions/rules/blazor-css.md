---
title: CSS Isolation in Blazor
impact: MEDIUM
impactDescription: Unscopeed CSS causes style conflicts across components
tags: blazor, css, isolation, deep
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

### Naming Convention

**Add a common prefix to all style names** to avoid conflicts with other projects or libraries:

```css
.my-project-popup {
    /* ... */
}
```

Reference: [Blazor CSS Isolation](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/css-isolation)

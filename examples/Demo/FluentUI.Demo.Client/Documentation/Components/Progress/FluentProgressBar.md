---
title: ProgressBar
route: /progress-bar
---

# ProgressBar

A **ProgressBar** provides a visual representation of content being loaded or processed.
This component is a wrapper for the web component.

## Best practices

**Do**
- Use an `indeterminate` **ProgressBar** (`Value=null`) when the total units 
  to completion is unknown.
- Provide a clear description of the progress operation
- Show text above and/or below the bar
- Combine steps of a single operation into one bar
- Use a `FluentField` to add a validation `Message` and hint message for the 
  indeterminate Bar when reduced-motion is active

**Don't**
- Use only a single word description
- Show text to the right or left of the bar
- 'Rewind' progress to show new steps

## Default

{{ ProgressBarDefault }}

## State and Color

The `State` attribute can be used to indicate a "brand" state (default),
`Error` state (red), `Warning` state (orange), or `Success` state (green).

The optional `Color` attribute can be used to set the color of the progress bar.
This color is overridden by the `State` attribute.

The background color of the progress bar can be set using the `BackgroundColor` attribute.

{{ ProgressBarState }}

## API FluentProgressBar

{{ API Type=FluentProgressBar }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentProgressBar }}
